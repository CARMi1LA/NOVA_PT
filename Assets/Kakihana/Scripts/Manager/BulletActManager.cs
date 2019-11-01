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
            new BulletData(_.Origintrans,_.shootForward,_.shootChara,_.bulletType);
        }).AddTo(this.gameObject);
    }

    // 攻撃モード処理
    public void BulletShootSet(Transform origin, BulletSetting.BulletList list, BulletManager.ShootChara chara, float interval)
    {
        float rot = 0;
        float radius;
        Vector3 moveForward = Vector3.zero;
        switch (list)
        {
            case BulletSetting.BulletList.Normal:
                switch (chara)
                {
                    case BulletManager.ShootChara.Player:
                        new BulletData(origin.position, origin.forward, chara, list);
                        break;
                    case BulletManager.ShootChara.Enemy:
                        moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                        //moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                        new BulletData(origin.position, origin.forward, chara, list);
                        break;
                }
                break;
            case BulletSetting.BulletList.Scatter:
                for (rot = 0; rot < 45; rot += 15)
                {
                    moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position,moveForward + offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
            case BulletSetting.BulletList.Fireworks:
                rot = 0.0f;
                for (int i = 0; i < 15; i ++)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    rot += 24.0f;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, moveForward + offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
            case BulletSetting.BulletList.Booster:
                moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                new BulletData(origin.position, origin.forward, chara, list);
                break;
            case BulletSetting.BulletList.None:
                break;
            case BulletSetting.BulletList.Whirlpool:
                rot = 0.0f;
                moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                this.UpdateAsObservable()
                    .Where(_ => rot < 360.0f)
                    .Sample(TimeSpan.FromSeconds(0.05f))
                    .Subscribe(_ =>
                    {
                        radius = rot * Mathf.Deg2Rad;
                        Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                        new BulletData(origin.position, offset, chara, list);
                        rot += 12.0f;
                    }).AddTo(this.gameObject);
                break;
            case BulletSetting.BulletList.Forrow:
                break;
            case BulletSetting.BulletList.WhirlScatterCombo:
                //for (int rot = 0; rot < deg; rot += 10)
                //{
                //}
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
