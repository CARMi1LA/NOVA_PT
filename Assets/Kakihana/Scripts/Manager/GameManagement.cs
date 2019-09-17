using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class GameManagement : GMSingleton<GameManagement>
{
    public Transform playerTrans;
    public Transform cameraTrans;
    public Vector3 cameraPos;
    public AIListManager listManager;

    public IntReactiveProperty gameLevel = new IntReactiveProperty(1);
    public IntReactiveProperty playerLevel = new IntReactiveProperty(1);
    public IntReactiveProperty playerScore = new IntReactiveProperty(0);

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
}
