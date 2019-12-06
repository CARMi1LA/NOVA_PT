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
        Debug.Log(bData.bPosition.ToString());

        // 返却フラグの設定
        isReturn.Value = false;
        // 移動処理の実行
        initTrigger.OnNext(bData);
        // 時限制で消滅
        Observable.Timer(System.TimeSpan.FromSeconds(bData.bDeathCount))
            .Subscribe(_ =>
            {
                isReturn.Value = true;

            }).AddTo(this.gameObject);
    }
}
