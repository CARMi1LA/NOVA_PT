using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

using Random = UnityEngine.Random;

public class BulletActManager : MonoBehaviour
{
    Subject<BulletData> bulletCreate = new Subject<BulletData>();
    // Start is called before the first frame update
    void Start()
    {
        bulletCreate.Subscribe(_ => 
        {
            new BulletData(_.Origintrans,_.shootChara,_.bulletType);
        }).AddTo(this.gameObject);
    }

    // 攻撃モード処理
    public void BulletShootSet(Transform origin, BulletSetting.BulletList list, BulletManager.ShootChara chara, float interval)
    {
        int deg = 360;
        float bulletSpeed = 20.0f;
        switch (list)
        {
            case BulletSetting.BulletList.Normal:
                new BulletData(origin, chara, list);
                //this.UpdateAsObservable()
                //    .Where(_ => GameManagement.Instance.isPause.Value == false)
                //    .Sample(TimeSpan.FromSeconds(interval))
                //    .Subscribe(_ => 
                //    {
                //        Debug.Log("Shoot");
                //        new BulletData(origin, chara, list);
                //    }).AddTo(this.gameObject);
                break;
            case BulletSetting.BulletList.Scatter:
                bulletCreate.OnNext(new BulletData(origin, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Normal));
                break;
            case BulletSetting.BulletList.Fireworks:
                break;
            case BulletSetting.BulletList.Booster:
                break;
            case BulletSetting.BulletList.None:
                break;
            case BulletSetting.BulletList.Whirlpool:
                for (int rot = 0; rot < deg; rot += 10)
                {
                }
                break;
            case BulletSetting.BulletList.Forrow:
                break;
            case BulletSetting.BulletList.WhirlScatterCombo:
                for (int rot = 0; rot < deg; rot += 10)
                {
                }
                break;
            case BulletSetting.BulletList.FireworksCombo:
                break;
            case BulletSetting.BulletList.UltMegaFireworks:
                break;
            case BulletSetting.BulletList.WhirlFireCombo:
                break;
            case BulletSetting.BulletList.BoostFireCombo:
                break;
            case BulletSetting.BulletList.WhirlBoostCombo:
                break;
            case BulletSetting.BulletList.Ultimate:
                break;
        }
    }
}
