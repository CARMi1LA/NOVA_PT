using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

using Random = UnityEngine.Random;

public class BulletActManager : MonoBehaviour
{
    // 弾生成Subject
    Subject<BulletData> bulletCreate = new Subject<BulletData>();

    Subject<BulletData> normalShoot = new Subject<BulletData>();
    // Start is called before the first frame update
    void Start()
    {
        bulletCreate.Subscribe(_ => 
        {
            new BulletData(_.Origintrans,_.shootForward,_.shootChara,_.bulletType);
        }).AddTo(this.gameObject);

        normalShoot.Subscribe(_ => 
        {
            Observable.FromCoroutine<BulletData>(x => EnemyShootNormal(_))
            .Subscribe(x => 
            {

            }).AddTo(this.gameObject);

        }).AddTo(this.gameObject);
    }

    // 攻撃モード処理
    public void BulletShootSet(Transform origin, BulletSetting.BulletList list, BulletManager.ShootChara chara, float interval)
    {
        // 3Wayなど多方向に向けて発射する際に使用
        float rot = 0;
        // 発射方向計算用ラジアン
        float radius;
        // 初期発射方向
        Vector3 moveForward = Vector3.zero;
        switch (list)
        {
            // 通常弾処理、撃つキャラクターによって処理を分ける
            case BulletSetting.BulletList.Normal:
                switch (chara)
                {
                    // プレイヤーの発射処理
                    case BulletManager.ShootChara.Player:
                        new BulletData(origin.position, origin.forward, chara, list);
                        break;
                    // 敵の発射処理
                    case BulletManager.ShootChara.Enemy:
                        moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                        new BulletData(origin.position, origin.forward, chara, list);
                        break;
                }
                break;
                // 拡散弾（3Way）処理
            case BulletSetting.BulletList.Scatter:
                float originRot;
                for (rot = -15; rot < 30; rot += 15)
                {
                    Vector3 vec = new Vector3(0, Mathf.Atan2(origin.forward.z, origin.forward.x) * Mathf.Rad2Deg, 0);
                    originRot = vec.y;
                    radius = (vec.y - rot) * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0,Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position,origin.forward + offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
                // 全方位弾（花火型）処理
            case BulletSetting.BulletList.Fireworks:
                moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                for (rot = 0; rot < 360; rot += 24)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    //rot += 24.0f;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
                // 加速弾処理
            case BulletSetting.BulletList.Booster:
                moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                new BulletData(origin.position, origin.forward, chara, list);
                break;
            case BulletSetting.BulletList.None:
                break;
                // うずまき弾処理
            case BulletSetting.BulletList.Whirlpool:
                rot = 0.0f;
                moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                //StartCoroutine(ShootWhirl(new BulletData(origin.position,origin.forward, chara, list)));
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
                // 追尾弾処理
            case BulletSetting.BulletList.Forrow:
                bulletCreate.OnNext(new BulletData(origin.position, origin.forward, chara, list));
                break;
            case BulletSetting.BulletList.WhirlScatterCombo:
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
                for (rot = 0; rot < 60; rot += 15)
                {
                    moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, moveForward + offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
            case BulletSetting.BulletList.FireworksCombo:
                for (rot = 0; rot < 360; rot += 90)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                for (rot = 0; rot < 360; rot += 24)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
            case BulletSetting.BulletList.UltMegaFireworks:
                for (rot = 0; rot < 360; rot += 12)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Scatter));
                }
                break;
            case BulletSetting.BulletList.WhirlFireCombo:
                rot = 0.0f;
                for (rot = 0; rot < 360; rot += 24)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, offset, BulletManager.ShootChara.Enemy, list));
                }
                moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                float rotB = 0.0f;
                this.UpdateAsObservable()
                    .Where(_ => rotB < 360.0f)
                    .Sample(TimeSpan.FromSeconds(0.05f))
                    .Subscribe(_ =>
                    {
                        radius = rotB * Mathf.Deg2Rad;
                        Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                        new BulletData(origin.position, offset, chara, list);
                        rotB += 12.0f;
                    }).AddTo(this.gameObject);
                break;
            case BulletSetting.BulletList.BoostFireCombo:
                for (rot = 0; rot < 360; rot += 24)
                {
                    moveForward = (origin.position - GameManagement.Instance.playerTrans.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Fireworks));
                }
                for (rot = 0; rot < 90; rot += 15)
                {
                    moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, moveForward + offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Booster));
                }
                break;
            case BulletSetting.BulletList.WhirlBoostCombo:
                for (rot = 0; rot < 90; rot += 15)
                {
                    moveForward = (GameManagement.Instance.playerTrans.position - origin.position).normalized;
                    radius = rot * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                    bulletCreate.OnNext(new BulletData(origin.position, moveForward + offset, BulletManager.ShootChara.Enemy, BulletSetting.BulletList.Booster));
                }
                rotB = 0.0f;
                this.UpdateAsObservable()
                    .Where(_ => rotB < 360.0f)
                    .Sample(TimeSpan.FromSeconds(0.05f))
                    .Subscribe(_ =>
                    {
                        radius = rotB * Mathf.Deg2Rad;
                        Vector3 offset = new Vector3(Mathf.Cos(radius), 0, Mathf.Sin(radius));
                        new BulletData(origin.position, offset, chara, list);
                        rotB += 12.0f;
                    }).AddTo(this.gameObject);
                break;
            case BulletSetting.BulletList.Ultimate:
                break;
        }
    }

    IEnumerator ShootWhirl(BulletData data)
    {
        float whirlRot = 0.0f;
        float whirlRad = 0.0f;

        for (int i = 0; i < 360; i+= 12)
        {
            whirlRad = i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(whirlRad), 0, Mathf.Sin(whirlRad));
            new BulletData(data.Origintrans, offset, data.shootChara, data.bulletType);
            whirlRot += 12.0f;
            Debug.Log(whirlRot);
            yield return new WaitForSeconds(0.05f);
        }

        //while (whirlRot <= 360.0f)
        //{
        //    whirlRad = whirlRot * Mathf.Deg2Rad;
        //    Vector3 offset = new Vector3(Mathf.Cos(whirlRad), 0, Mathf.Sin(whirlRad));
        //    new BulletData(data.Origintrans, offset, data.shootChara, data.bulletType);
        //    whirlRot += 12.0f;
        //    Debug.Log(whirlRot);
        //    yield return new WaitForSeconds(0.05f);
        //}
        yield break;
    }

    IEnumerator EnemyShootNormal(BulletData data)
    {

        yield return new WaitForSeconds(0.1f);
    }

}
