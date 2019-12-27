using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class TDEnemyUnit : MonoBehaviour
{
    // エネミーのユニットの管理
    [SerializeField]
    public TDEnemyManager eManager;

    [SerializeField]
    bool isCoreUnit = false;

    // ヘルス
    public IntReactiveProperty eHealth = new IntReactiveProperty(100);

    // ダメージ処理
    public Subject<Unit> DamageTrigger = new Subject<Unit>();
    // 死亡処理
    public Subject<Unit> DeathTrigger = new Subject<Unit>();
    private void Awake()
    {
        // ユニットの初期化
        eManager.InitTrigger
            .Do(value =>        // 通常ユニットの場合
            {
                Debug.Log("HP変更" + value.eHealth);
                eHealth.Value = value.eHealth;
            })
            .Where(x => isCoreUnit)
            .Subscribe(value => // コアユニットの場合
            {
                Debug.Log("HP変更" + value.eCoreHealth);
                eHealth.Value = value.eCoreHealth;
            })
            .AddTo(this.gameObject);
    }
    void Start()
    {
        // ヘルスが0になった時の処理
        this.UpdateAsObservable()
            .Where(x => eHealth.Value == 0)
            .Subscribe(_ =>
            {
                Debug.Log("あぷでーと");

                if (isCoreUnit)
                {
                    Debug.Log("ちょくし");

                    eManager.CoreDeathTrigger.OnNext(Unit.Default);
                }
                else
                {
                    DeathTrigger.OnNext(Unit.Default);
                }

            }).AddTo(this.gameObject);

        Debug.Log("さいごまで読んでる？");


        // 直死
        eManager.CoreDeathTrigger
            .Subscribe(_ =>
            {
                DeathTrigger.OnNext(Unit.Default);

            }).AddTo(this.gameObject);
    }
}
