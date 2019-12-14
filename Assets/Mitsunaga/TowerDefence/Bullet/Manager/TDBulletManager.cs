using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDBulletManager : MonoBehaviour
{
    // Bulletの挙動を統括する

    // Bullet返却のフラグ
    // 他Objectなどと衝突したり、一定時間が経過するとこのフラグが立つ
    public BoolReactiveProperty isReturn = new BoolReactiveProperty(false);

    public Subject<TDBulletData> initTrigger = new Subject<TDBulletData>();

    void Start()
    {
        // Bullet返却
        isReturn
            .Where(x => isReturn.Value)
            .Subscribe(_ =>
            {
                TDBulletSpawner.Instance.bulletReturnSubject.OnNext(this);

            }).AddTo(this.gameObject);
    }

    // 初期化
    public void Init(TDBulletData bData)
    {
        // 返却フラグの設定
        isReturn.Value = false;
        // 移動処理の実行
        initTrigger.OnNext(bData);
        // 時限制で消滅
        var Return = Observable.EveryUpdate()
            .Where(x => isReturn.Value);

        Observable.Timer(System.TimeSpan.FromSeconds(bData.bDeathCount))
            .TakeUntil(Return)
            .Subscribe(_ =>
            {
                Debug.Log("時間で消滅");
                isReturn.Value = true;

            }).AddTo(this.gameObject);
    }
}
