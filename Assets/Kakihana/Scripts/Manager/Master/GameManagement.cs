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
    public TowerManager redTw, blueTw, yellowTw, greenTw;

    [SerializeField] public InputValueData1P valueData1P;
    [SerializeField] public InputValueData2P valueData2P;

    public AudioSource[] bgms;

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
    public IntReactiveProperty mater = new IntReactiveProperty(0);
    // 達成率、評価に影響
    [SerializeField] float achievementRate = 0.0f;

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
    // ポーズフラグ
    public BoolReactiveProperty isPause = new BoolReactiveProperty(true);
    public BoolReactiveProperty playerUlt = new BoolReactiveProperty(false);
    public BoolReactiveProperty enemyUlt = new BoolReactiveProperty(false);

    // ゲームステートプロパティ
    public ReactiveProperty<BattleMode> gameState = new ReactiveProperty<BattleMode>();

    // 敵情報追加用Subject
    public Subject<Transform> enemyInfoAdd = new Subject<Transform>();
    // マター取得Subject
    public Subject<int> addMater = new Subject<int>();

    protected override void Awake()
    {
        base.Awake();
        // プレイヤーデータの初期化
        tdPlaerData = new TDPlayerData();
        // カメラ座標の取得
        cameraPos = cameraTrans.position;

        for (int i = 0; i > players.Length; i++)
        {
            gameInput.InitSubject.OnNext(i);
        }

        gameHUD.ScoreRP.Value = gameScore.Value;
        masterDataList = Resources.Load<MasterDataList>("MasterDataList");
        masterData = masterDataList.masterDataList[0];
        gameState.Value = BattleMode.Wait;
        targetTw = twList[Random.Range(0, twList.Length)];
    }

    void Start()
    {
        addMater.Subscribe(value => 
        {
            mater.Value = value;
        }).AddTo(this.gameObject);

        gameState
            .Where(_ => gameState.Value == BattleMode.Wait && waveSettingFlg.Value == false)
            .Subscribe(_ => 
            {
                if (waveNum.Value == 0)
                {
                    masterTime = masterData.waitTime * 2;
                    targetTw = twList[Random.Range(0, twList.Length)];
                }
                else
                {
                    masterTime = masterData.waitTime;
                    targetTw = twList[Random.Range(0, twList.Length)];
                }
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => waveSettingFlg.Value == true)
            .Where(_ => masterTime >= 0 && gameState.Value == BattleMode.Wait)
            .Subscribe(_ => 
            {
                masterTime -= Time.deltaTime;
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
            gameHUD.ScoreRP.Value = gameScore.Value;
        }).AddTo(this.gameObject);

        playerUlt.Where(_ => playerUlt.Value == true)
            .Sample(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => 
            {
                playerUlt.Value = false;
            }).AddTo(this.gameObject);

        enemyUlt.Where(_ => enemyUlt.Value == true)
        .Sample(TimeSpan.FromSeconds(1.0f))
        .Subscribe(_ =>
        {
            enemyUlt.Value = false;
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
