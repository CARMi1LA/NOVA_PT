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
    [SerializeField] IntReactiveProperty gameLevel = new IntReactiveProperty(1);
    // プレイヤーレベル、上がるほどスコア上昇率に影響する
    [SerializeField] IntReactiveProperty playerLevel = new IntReactiveProperty(1);
    // スコア、達成率に影響
    public IntReactiveProperty playerScore = new IntReactiveProperty(0);
    // コンボ数、ダメージを与えるたびに上昇
    [SerializeField] IntReactiveProperty combo = new IntReactiveProperty(0);
    // 最大コンボ数
    [SerializeField] IntReactiveProperty maxCombo = new IntReactiveProperty(0);
    // コンボが途切れるまでのタイマー
    [SerializeField] IntReactiveProperty comboResetCount = new IntReactiveProperty(0);
    // コンボが途切れるまでの制限時間
    [SerializeField] IntReactiveProperty comboResetLimit = new IntReactiveProperty(0);
    // 達成率、評価に影響
    [SerializeField] float achievementRate = 0.0f;

    public BoolReactiveProperty isClear = new BoolReactiveProperty(false);
    public BoolReactiveProperty gameOver = new BoolReactiveProperty(false);
    public BoolReactiveProperty isPause = new BoolReactiveProperty(true);
    public BoolReactiveProperty onClick = new BoolReactiveProperty(false);


    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
        cameraPos = cameraTrans.position;

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
}
