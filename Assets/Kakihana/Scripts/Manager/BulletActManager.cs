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
            new BulletData(_.bulletSpeed, _.Origintrans, _.shootChara, _.bulletType);
        }).AddTo(this.gameObject);
    }

    // 攻撃モード処理
    public void EnemyAtkCalc(Transform origin, BulletSetting.BulletList list, float angle,float interval)
    {
        int deg = 360;
        float bulletSpeed = 20.0f;
        switch (list)
        {
            case BulletSetting.BulletList.Normal:
                bulletCreate.OnNext(new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.None, 0.0f, Random.Range(0, 360)));
                break;
            case BulletSetting.BulletList.Scatter:
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 0.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 30.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -30.0f, 0.0f);
                break;
            case BulletSetting.BulletList.Fireworks:
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 0.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 45.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -45.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 90.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -90.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 135.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -135.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 180.0f, 0.0f);
                break;
            case BulletSetting.BulletList.Booster:
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 0.0f, 0.0f);
                break;
            case BulletSetting.BulletList.None:
                break;
            case BulletSetting.BulletList.Whirlpool:
                for (int rot = 0; rot < deg; rot += 10)
                {
                    new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, rot, 0.0f);
                }
                break;
            case BulletSetting.BulletList.Forrow:
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 0.0f, 0.0f);
                break;
            case BulletSetting.BulletList.WhirlScatterCombo:
                for (int rot = 0; rot < deg; rot += 10)
                {
                    new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, rot, 0.0f);
                }
                break;
            case BulletSetting.BulletList.FireworksCombo:
                new BulletData(bulletSpeed * 2, origin, BulletManager.ShootChara.Enemy, actID, 0.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 45.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -45.0f, 0.0f);
                new BulletData(bulletSpeed * 2, origin, BulletManager.ShootChara.Enemy, actID, 90.0f, 0.0f);
                new BulletData(bulletSpeed * 2, origin, BulletManager.ShootChara.Enemy, actID, -90.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 135.0f, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -135.0f, 0.0f);
                new BulletData(bulletSpeed * 2, origin, BulletManager.ShootChara.Enemy, actID, 180.0f, 0.0f);
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
