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

    // ゲームレベル、上がるほど敵が強化される
    [SerializeField] public IntReactiveProperty gameLevel = new IntReactiveProperty(1);
    // プレイヤーレベル、上がるほどスコア上昇率に影響する
    [SerializeField] public IntReactiveProperty playerLevel = new IntReactiveProperty(1);
    // スコア、達成率に影響
    public IntReactiveProperty gameScore = new IntReactiveProperty(0);
    // コンボ数、攻撃がヒットする度に上昇
    // 被弾し、HPが減ったときにリセット、バリア減少ではリセットされない
    [SerializeField] public IntReactiveProperty combo = new IntReactiveProperty(0);
    // 最大コンボ数
    [SerializeField] IntReactiveProperty maxCombo = new IntReactiveProperty(0);
    // 達成率、評価に影響
    [SerializeField] float achievementRate = 0.0f;

    // 初回起動完了したか
    public BoolReactiveProperty starting = new BoolReactiveProperty(false);

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

        isPause.Value = false;
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
