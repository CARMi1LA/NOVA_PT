using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class GameManagement : GMSingleton<GameManagement>
{
    // ゲームシステムクラス

    public Transform playerTrans;           // プレイヤーの座標
    public Transform cameraTrans;           // カメラの座標
    public Vector3 cameraPos;               // カメラ移動量
    public AIListManager listManager;       // AI行動名リスト
    public AI_ActManager actManager;
    public HUD_Model gameHUD;

    // ゲームレベル、上がるほど敵が強化される
    [SerializeField] public IntReactiveProperty gameLevel = new IntReactiveProperty(1);
    // プレイヤーレベル、上がるほどスコア上昇率に影響する
    [SerializeField] public IntReactiveProperty playerLevel = new IntReactiveProperty(1);
    // スコア、達成率に影響
    public IntReactiveProperty gameScore = new IntReactiveProperty(0);
    // ダメージボーナス、攻撃がヒットする度に貰える
    public int damageBonus;
    // 撃破ボーナス、敵を撃破すると貰える
    public int enemyDeathBonus;
    // コンボ数、攻撃がヒットする度に上昇
    // 被弾し、HPが減ったときにリセット、バリア減少ではリセットされない
    [SerializeField] public IntReactiveProperty combo = new IntReactiveProperty(0);
    // 最大コンボ数
    [SerializeField] IntReactiveProperty maxCombo = new IntReactiveProperty(0);
    // 敵撃破コンボ数
    [SerializeField] public IntReactiveProperty enemyDeathCombo = new IntReactiveProperty(0);
    // 攻撃コンボ数
    [SerializeField] IntReactiveProperty attackCombo = new IntReactiveProperty(0);
    // 攻撃コンボ時、取得できるスコアの倍率
    [SerializeField] IntReactiveProperty attackScoreMul = new IntReactiveProperty(0);
    // 達成率、評価に影響
    [SerializeField] float achievementRate = 0.0f;
    // 攻撃コンボリセットカウント
    [SerializeField] float atkComboResetCount = 0.0f;
    // 攻撃コンボリセット時間
    [SerializeField] float atkComboResetLimit = 10.0f;

    // 初回起動完了したか
    public BoolReactiveProperty starting = new BoolReactiveProperty(false);

    // ゲームクリアフラグ
    public BoolReactiveProperty isClear = new BoolReactiveProperty(false);
    // ゲームオーバーフラグ
    public BoolReactiveProperty gameOver = new BoolReactiveProperty(false);
    // ポーズフラグ
    public BoolReactiveProperty isPause = new BoolReactiveProperty(true);
    public BoolReactiveProperty onClick = new BoolReactiveProperty(false);


    protected override void Awake()
    {
        base.Awake();

        // マスタークラスが勝手に削除されないように設定
        DontDestroyOnLoad(this.gameObject);
        // カメラ座標の取得
        cameraPos = cameraTrans.position;

        isPause.Value = false;

        gameHUD.ScoreRP.Value = gameScore.Value;
        gameHUD.ComboRP.Value = combo.Value;
    }

    void Start()
    {
        starting.Where(s => s == false)
       .Subscribe(s =>
       {
           isPause.Value = true;
       }).AddTo(this.gameObject);

        starting.Where(s => s == true && isPause.Value == false)
            .Subscribe(s =>
            {
                isClear.Where(x => x).
                Subscribe(_ =>
                {
                    // クリア処理
                    gameHUD.maxCombo = maxCombo.Value;
                }).AddTo(this.gameObject);

                gameOver
                .Where(x => x)
                .Where(x => !isClear.Value)
                .Subscribe(_ =>
                {
                    //ゲームオーバー処理
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
        .Where(_ => starting.Value == false)
        .Subscribe(_ =>
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                starting.Value = true;
            }
        }).AddTo(this.gameObject);

        attackCombo.Where(_ => attackCombo.Value >= 1)
            .Subscribe(_ =>
            {
                attackScoreMul.Value = 1;
                atkComboResetCount += Time.deltaTime;
                if (atkComboResetCount >= atkComboResetLimit)
                {
                    attackCombo.Value = 0;
                }
            }).AddTo(this.gameObject);

        attackCombo.Where(_ => attackCombo.Value >= 100).Subscribe(_ => 
        {
            attackScoreMul.Value = 2;
        }).AddTo(this.gameObject);

        attackCombo.Where(_ => attackCombo.Value >= 200).Subscribe(_ =>
        {
            attackScoreMul.Value = 3;
        }).AddTo(this.gameObject);

        attackCombo.Where(_ => attackCombo.Value >= 300).Subscribe(_ =>
        {
            attackScoreMul.Value = 4;
        }).AddTo(this.gameObject);

        attackCombo.Where(_ => attackCombo.Value >= 500).Subscribe(_ =>
        {
            attackScoreMul.Value = 5;
        }).AddTo(this.gameObject);

        gameScore.Subscribe(_ =>
        {
            gameHUD.ScoreRP.Value = gameScore.Value;
        }).AddTo(this.gameObject);

        combo.Subscribe(_ =>
        {
            gameHUD.ComboRP.Value = combo.Value;
        }).AddTo(this.gameObject);

        maxCombo.Where(_ => combo.Value >= maxCombo.Value)
            .Subscribe(_ => 
            {
                maxCombo.Value = combo.Value;
            }).AddTo(this.gameObject);
    }

    public void HitCombo()
    {
        combo.Value++;
    }

    // ダメージボーナス処理
    public void DamageScore()
    {
        // コンボを加算
        attackCombo.Value++;
        // コンボリセットまでの時間を0に
        atkComboResetCount = 0.0f;
        // ダメージボーナスと連続攻撃に応じて追加でスコアを加算
        gameScore.Value += damageBonus * attackScoreMul.Value;
    }

    // 敵撃破ボーナス処理
    public void DestoyScore()
    {
        // 敵撃破を加算
        enemyDeathCombo.Value++;
        // スコアを加算、被弾せずに撃破し続けると更に加算
        gameScore.Value += enemyDeathBonus * enemyDeathCombo.Value;
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
