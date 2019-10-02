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

    // ゲームレベル、上がるほど敵が強化される
    [SerializeField] public IntReactiveProperty gameLevel = new IntReactiveProperty(1);
    // プレイヤーレベル、上がるほどスコア上昇率に影響する
    [SerializeField] public IntReactiveProperty playerLevel = new IntReactiveProperty(1);
    // スコア、達成率に影響
    public IntReactiveProperty playerScore = new IntReactiveProperty(0);
    // コンボ数、攻撃がヒットする度に上昇
    // 被弾し、HPが減ったときにリセット、バリア減少ではリセットされない
    [SerializeField] public IntReactiveProperty combo = new IntReactiveProperty(0);
    // 最大コンボ数
    [SerializeField] IntReactiveProperty maxCombo = new IntReactiveProperty(0);
    // 達成率、評価に影響
    [SerializeField] float achievementRate = 0.0f;

    // 初回起動完了したか
    bool starting = false;

    public BoolReactiveProperty isClear = new BoolReactiveProperty(false);
    public BoolReactiveProperty gameOver = new BoolReactiveProperty(false);
    public BoolReactiveProperty isPause = new BoolReactiveProperty(true);
    public BoolReactiveProperty onClick = new BoolReactiveProperty(false);


    protected override void Awake()
    {
        base.Awake();

        // マスタークラスが勝手に削除されないように設定
        DontDestroyOnLoad(this.gameObject);
        // カメラ座標の取得
        cameraPos = cameraTrans.position;

        // ステージ開始時、ポーズ状態に、クリックで解除
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && onClick.Value == false)
            .Subscribe(_ => 
            {
                onClick.Value = true;
            }).AddTo(this.gameObject);

        // クリックが押されたらポーズを解除
        onClick.Where(_ => _ == true && starting == false)
            .Subscribe(_ => 
             {
                isPause.Value = false;
                starting = true;
            }).AddTo(this.gameObject);

        isPause.Where(_ => true && starting == true)
            .Subscribe(_ =>
            {
                // ポーズ処理
                
            }).AddTo(this.gameObject);

        isPause.Where(_ => false && starting == true)
            .Subscribe(_ => 
            {

            }).AddTo(this.gameObject);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause.Value = true;
        }

        isClear.Where(x => x).
            Subscribe(_ =>
            {
                // クリア処理
            }).AddTo(this.gameObject);

        gameOver
            .Where(x => x)
            .Where(x => !isClear.Value)
            .Subscribe(_ =>
            {
                //ゲームオーバー処理
            }).AddTo(this.gameObject);

        maxCombo.Where(_ => combo.Value >= _).Subscribe(_ =>
        {
            maxCombo.Value = combo.Value;
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
