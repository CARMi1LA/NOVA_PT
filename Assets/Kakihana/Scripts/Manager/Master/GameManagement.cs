using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UniRx;
using UniRx.Triggers;

// Unity側のランダム関数を使用
using Random = UnityEngine.Random;
public class GameManagement : GMSingleton<GameManagement>
{
    public enum BattleMode
    {
        Wait = 0,
        Attack = 1,
        Clear = 2,
        GameOver = 3
    }
    // ゲームシステムクラス

    public Transform playerTrans;           // プレイヤーの座標
    public Transform cameraTrans;           // カメラの座標
    public Transform centerTrans;           // 中心座標
    public Vector3 cameraPos;               // カメラ移動量
    public Vector3 cScreen;
    public Vector3 cWorld;
    public AI_ActManager actManager;
    public HUD_Model gameHUD;
    public Result_Model resultUI;
    public BulletActManager bulletActManager;
    public PlayerManager[] players;
    public GameInputManager gameInput;
    public TDPlayerData tdPlaerData;
    public MasterData masterData;
    public MasterDataList masterDataList;
    public EnemyInfoList enemyInfoList;
    public TowerManager redTw, blueTw, yellowTw, greenTw;
    public TowerManager[] towerM;
    public TD_GameOverUI tdMainUI;

    [SerializeField] public InputValueData1P valueData1P;
    [SerializeField] public InputValueData2P valueData2P;

    public AudioSource gameBgm;
    public AudioClip[] bgms;

    public MasterData.TowerColor[] twList;
    public MasterData.TowerColor targetTw;
    public float masterTime = 0.0f;

    // 現在のウェーブ
    public IntReactiveProperty waveNum = new IntReactiveProperty(0);
    // タワーの生存数
    public IntReactiveProperty towerAliveNum = new IntReactiveProperty(4);
    // スコア、達成率に影響
    public IntReactiveProperty gameScore = new IntReactiveProperty(0);
    // 所持マター、タワーや自キャラの強化が可能
    public IntReactiveProperty mater = new IntReactiveProperty(-1);
    // 達成率、評価に影響
    [SerializeField] float achievementRate = 0.0f;
    // ショップに入った事があるか
    public bool firstShopWindow = false;

    // 初回起動完了したか
    public BoolReactiveProperty starting = new BoolReactiveProperty(false);
    // ウェーブ進行可能か
    public BoolReactiveProperty waveSettingFlg = new BoolReactiveProperty(false);
    // デバッグモード
    public BoolReactiveProperty isDebug = new BoolReactiveProperty(false);
    // ゲームクリアフラグ
    public BoolReactiveProperty isClear = new BoolReactiveProperty(false);
    // ゲームオーバーフラグ
    public BoolReactiveProperty gameOver = new BoolReactiveProperty(false);
    // ゲームクリアフラグ
    public BoolReactiveProperty tdGameClear = new BoolReactiveProperty(false);
    // ポーズフラグ
    public BoolReactiveProperty isPause = new BoolReactiveProperty(true);
    // ショップCanvas表示フラグ
    public BoolReactiveProperty shopCanvasEnable = new BoolReactiveProperty(false);
    // タワーバフフラグ
    public BoolReactiveProperty[] towerUltFlg = new BoolReactiveProperty[4];
    // タワー死亡フラグ
    public BoolReactiveProperty[] towerDeaths = new BoolReactiveProperty[4];
    // タイトルバックフラグ
    public BoolReactiveProperty titleBackFlg = new BoolReactiveProperty(false);

    // ゲームステートプロパティ
    public ReactiveProperty<BattleMode> gameState = new ReactiveProperty<BattleMode>();
    // ショップのCanvas
    public CanvasGroup shopCanvas;

    // 敵情報追加用Subject
    public Subject<Transform> enemyInfoAdd = new Subject<Transform>();
    // ゲーム時間強制進行用Subject
    public Subject<Unit> masterTimeSkip = new Subject<Unit>();
    // ショップ入店Subject
    public Subject<Unit> shopInSub = new Subject<Unit>();
    // ショップ退出Subject
    public Subject<Unit> shopOutSub = new Subject<Unit>();
    // マター追加Subject（デバッグ用）
    public Subject<int> addMaterDebug = new Subject<int>();
    // マター取得Subject
    public Subject<int> addMater = new Subject<int>();
    // 戦闘モード進行用Subject
    public Subject<Unit> battleModeSub = new Subject<Unit>();
    // タワー消滅Subject
    public Subject<MasterData.TowerColor> towerDeathSub = new Subject<MasterData.TowerColor>();
    // ゲームオーバーSubject
    public Subject<Unit> gameOverSub = new Subject<Unit>();
    // ゲームクリアSubject
    public Subject<Unit> gameClear = new Subject<Unit>();
    // （デバッグ用）タイトルバックSubject
    public Subject<Unit> titleBack = new Subject<Unit>();
    // ボス出現検知用Subject
    public Subject<TDEnemyUnit> bossSpawn = new Subject<TDEnemyUnit>();
    // タワーバフフラグONSubject
    public Subject<ShopData.TowerColor> towerUltOn = new Subject<ShopData.TowerColor>();
    // タワーバフフラグOFFSubject
    public Subject<ShopData.TowerColor> towerUltOff = new Subject<ShopData.TowerColor>();
    // 初期化用Subject
    public Subject<Unit> masterInit = new Subject<Unit>();


    protected override void Awake()
    {
        base.Awake();
        masterInit.Subscribe(_ => 
        {
            mater.Value += 3000;
            ShopManager.Instance.shopUpdate.OnNext(Unit.Default);
        }).AddTo(this.gameObject);
        // プレイヤーデータの初期化
        tdPlaerData = new TDPlayerData();
        // カメラ座標の取得
        cameraPos = cameraTrans.position;
        // 入力スクリプト初期化
        for (int i = 0; i > players.Length; i++)
        {
            gameInput.InitSubject.OnNext(i);
        }
        // マスタデータリストの取得
        masterDataList = Resources.Load<MasterDataList>("MasterDataList");
        // マスタデータの取得
        masterData = masterDataList.masterDataList[0];
        // ゲームステートの設定
        gameState.Value = BattleMode.Wait;
        // ショップウィンドウは初期では非表示に
        shopCanvas.alpha = 0;
    }

    void Start()
    {
        masterInit.OnNext(Unit.Default);
        // マター獲得処理
        addMater.Subscribe(value => 
        {
            mater.Value += value;
        }).AddTo(this.gameObject);

        // マター獲得処理（デバッグ用）
        addMaterDebug.Subscribe(value =>
        {
            mater.Value += value;
        }).AddTo(this.gameObject);

        // ゲーム時間スキップ処理
        masterTimeSkip.Subscribe(_ => 
        {
            masterTime = 3.0f;
        }).AddTo(this.gameObject);

        // 入店処理
        shopInSub
            .Subscribe(_ => 
            {
                shopCanvas.alpha = 1.0f;
                ShopManager.Instance.shopUpdate.OnNext(Unit.Default);
                shopCanvasEnable.Value = true;
                shopCanvas.blocksRaycasts = true;
            }).AddTo(this.gameObject);

        // 退出処理
        shopOutSub
            .Subscribe(_ => 
            {
                shopCanvas.alpha = 0.0f;
                shopCanvasEnable.Value = false;
                shopCanvas.blocksRaycasts = false;
            }).AddTo(this.gameObject);

        // タワー消滅処理
        towerDeathSub.Subscribe(val => 
        {
            towerDeaths[(int)val].Value = true;
            towerAliveNum.Value--;
        }).AddTo(this.gameObject);

        towerUltOn.Subscribe(value => 
        {
            towerUltFlg[(int)value].Value = true;
        }).AddTo(this.gameObject);

        towerUltOff.Subscribe(value =>
        {
            towerUltFlg[(int)value].Value = false;
        }).AddTo(this.gameObject);

        titleBack.Subscribe(_ => 
        {
            SceneManager.LoadScene("00 Title");
        }).AddTo(this.gameObject);

        // メインフェードイン
        this.UpdateAsObservable()
            .Where(_ => tdMainUI.mainFade.alpha >= 0.01f && gameOver.Value == false)
            .Subscribe(_ => 
            {
                tdMainUI.mainFadeIn.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // フェードアウト＆タイトルバック
        this.UpdateAsObservable()
            .Where(_ => titleBackFlg.Value == true)
            .Do(_ =>
            {
                if (tdMainUI.mainFade.alpha < 1)
                {
                    tdMainUI.mainFadeOut.OnNext(Unit.Default);
                }
            })
            .Delay(System.TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => 
            {
                titleBack.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // ゲームオーバー処理
        this.UpdateAsObservable()
            .Where(_ => gameOver.Value == true)
            .Do(_ =>
            {
                if (tdMainUI.gameOverFade.alpha <= 0.5f)
                {
                    tdMainUI.gameOverFadeOut.OnNext(Unit.Default);
                }
            })
            .Delay(System.TimeSpan.FromSeconds(0.5f))
            .Do(_ =>
            {
                if (tdMainUI.gameOverUI.alpha < 1)
                {
                    tdMainUI.gmOverUIFadeOut.OnNext(Unit.Default);
                }
            }).Delay(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => 
            {
                if (Input.GetButton("Button_A") == true || Input.GetButtonDown("Fire1") == true)
                {
                    titleBackFlg.Value = true;
                }
            }).AddTo(this.gameObject);

        // ゲームクリア処理
        this.UpdateAsObservable()
            .Where(_ => tdGameClear.Value == true)
            .Do(_ =>
            {
                if (tdMainUI.clearFade.alpha <= 0.5f)
                {
                    tdMainUI.clearFadeOut.OnNext(Unit.Default);
                }
            }).Delay(System.TimeSpan.FromSeconds(0.5f))
            .Do(_ =>
            {
                tdMainUI.aliveText.text = string.Format("TowerAlive:{0}", towerAliveNum);
                switch (towerAliveNum.Value)
                {
                    case 1:
                        tdMainUI.rankText.text = string.Format("C");
                        break;
                    case 2:
                        tdMainUI.rankText.text = string.Format("B");
                        break;
                    case 3:
                        tdMainUI.rankText.text = string.Format("A");
                        break;
                    case 4:
                        tdMainUI.rankText.text = string.Format("S");
                        break;
                    default:
                        break;
                }
                if (tdMainUI.clearUI.alpha <= 1)
                {
                    tdMainUI.clearUIFadeOut.OnNext(Unit.Default);
                }
            }).Delay(System.TimeSpan.FromSeconds(0.5f))
            .Do(_ =>
            {
                if (tdMainUI.scoreUI.alpha < 1)
                {
                    tdMainUI.scoreUIFadeOut.OnNext(Unit.Default);
                }
            }).Delay(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => 
            {
                if (Input.GetButton("Button_A") == true || Input.GetButtonDown("Fire1") == true)
                {
                    titleBackFlg.Value = true;
                }
            }).AddTo(this.gameObject);

        // デバッグ用、F12キーを押すとデバッグモードへ
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F12) && isDebug.Value == false)
            .Subscribe(_ =>
            {
                //isDebug.Value = true;
            }).AddTo(this.gameObject);

        // デバッグモード状態でもう一度F12キーを押すと解除
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F12) && isDebug.Value == true)
            .Subscribe(_ =>
            {
                //isDebug.Value = false;
            }).AddTo(this.gameObject);

        // デバッグ用F11キーでタイトルバック
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F11) && isDebug.Value == true)
            .Subscribe(_ =>
            {
                titleBack.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // デバッグ用、F1キーを押すと進行時間の短縮が可能
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F1) && isDebug.Value == true)
            .Subscribe(_ => 
            {
                masterTime = 3.0f;
            }).AddTo(this.gameObject);

        // デバッグ用、ショップ入店処理
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F2) && isDebug.Value == true)
            .Subscribe(_ =>
            {
                shopInSub.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // デバッグ用、ショップ退出処理
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F3) && isDebug.Value == true)
            .Subscribe(_ =>
            {
                shopOutSub.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // 1万マターを即取得する（デバッグ用)
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.F4) && isDebug.Value == true)
            .Subscribe(_ => 
            {
                addMaterDebug.OnNext(10000);
            }).AddTo(this.gameObject);

        // 待機モード時の準備処理
        gameState
            .Where(_ => gameState.Value == BattleMode.Wait && waveSettingFlg.Value == false)
            .Subscribe(_ => 
            {
                gameBgm.Stop();
                // 初回のみ待機時間を倍にする
                if (waveNum.Value == 0)
                {
                    masterTime = masterData.waitTime * 2;
                    // 準備完了通知
                    waveSettingFlg.Value = true;
                    gameBgm.clip = bgms[0];
                    gameBgm.Play();
                }
                else
                {
                    // 待機時間を設定
                    masterTime = masterData.waitTime;
                    // 準備完了通知
                    waveSettingFlg.Value = true;
                    gameBgm.clip = bgms[0];
                    gameBgm.Play();
                }
            }).AddTo(this.gameObject);

        // 待機モード時の処理
        this.UpdateAsObservable()
            .Where(_ => waveSettingFlg.Value == true)
            .Do(_ =>
            {
                // 準備完了なら実行、設定された待機時間をカウント
                masterTime -= Time.deltaTime;
            })
            .Where(_ => masterTime <= 0 && gameState.Value == BattleMode.Wait)
            .Subscribe(_ => 
            {
                // 設定時間経過後、戦闘モードへ
                // ウェーブを進行させる
                waveNum.Value++;
                // 戦闘モードへ移行
                gameState.Value = BattleMode.Attack;
                // 準備完了状態を解除
                waveSettingFlg.Value = false;
                // 戦闘時間の設定
                masterTime = masterData.waveTime[waveNum.Value];
            }).AddTo(this.gameObject);

        gameState.Where(_ => gameState.Value == BattleMode.Attack)
            .Subscribe(_ => 
            {
                gameBgm.Stop();
                if (waveNum.Value == masterData.waveTime.Length)
                {
                    gameBgm.clip = bgms[2];
                    gameBgm.Play();
                }
                else
                {
                    gameBgm.clip = bgms[1];
                    gameBgm.Play();
                }
            }).AddTo(this.gameObject);

        // 戦闘モード時の処理
        this.UpdateAsObservable()
            .Where(_ => waveSettingFlg.Value == false)
            .Do(_ =>
            {
                // 戦闘時間をカウント
                masterTime -= Time.deltaTime;
            })
            .Where(_ => masterTime <= 0 && gameState.Value == BattleMode.Attack)
            .Subscribe(_ =>
            {
                // 設定時間経過後、待機モードへ
                gameState.Value = BattleMode.Wait;
            }).AddTo(this.gameObject);

        towerAliveNum.Where(_ => towerAliveNum.Value <= 0)
            .Subscribe(_ => 
            {
                
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => SceneManager.GetActiveScene().name == "00 Title")
            .Subscribe(_ =>
            {
                isPause.Value = false;
                isClear.Value = false;
                starting.Value = false;
                StageManager.Instance.startingFlg.Value = false;
            }).AddTo(this.gameObject);

        starting.Where(s => s == false)
            .Subscribe(s =>
            {
                   isPause.Value = true;
            }).AddTo(this.gameObject);

        starting.Where(s => s == true && isPause.Value == false)
            .Subscribe(s =>
            {
                isClear.Where(x => isClear.Value == true && isDebug.Value == false).
                Subscribe(_ =>
                {
                    resultUI.setState(Result_Model.GAMESTATE.CLEAR);
                }).AddTo(this.gameObject);

                gameOver
                .Where(x => x)
                .Where(x => !isClear.Value)
                .Subscribe(_ =>
                {
                    //ゲームオーバー処理
                    resultUI.setState(Result_Model.GAMESTATE.GAMEOVER);
                }).AddTo(this.gameObject);


            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => isPause.Value == true)
            .Subscribe(_ =>
            {
                if (Input.GetMouseButtonDown(0) == true || Input.GetKey(KeyCode.F1))
                {
                    isPause.Value = false;
                }
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => isPause.Value == false)
            .Subscribe(_ => 
            {
                // マウスのスクリーン座標を取得
                cScreen = Input.mousePosition;
                // カメラの焦点を補正
                cScreen.z = 50.0f;
                // スクリーン座標からワールド座標へ変換
                cWorld = Camera.main.ScreenToWorldPoint(cScreen);
            }).AddTo(this.gameObject);
        this.UpdateAsObservable()
        .Where(_ => starting.Value == false)
        .Subscribe(_ =>
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                starting.Value = true;
            }
        }).AddTo(this.gameObject);

        gameScore.Where(_ => isClear.Value == false)
            .Subscribe(_ =>
        {
            // gameHUD.ScoreRP.Value = gameScore.Value;
        }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => shopCanvasEnable.Value == true)
            .Subscribe(_ =>
            {
                shopCanvas.alpha = 1.0f;
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => shopCanvasEnable.Value == false)
            .Subscribe(_ =>
            {
                shopCanvas.alpha = 0.0f;
            }).AddTo(this.gameObject);
    }

    // 次シーン移行処理
    public void NextStage()
    {

    }

    // ゲームオーバー時、コンティニューされたときの処理
    public void GameContinue()
    {

    }

    // クリア処理
    public void Result()
    {

    }

    // シーン移動処理
    public void SceneMove(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
