using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData
{
    public float bulletSpeed;     // 速度
    public float bulletRot;       // 角度
    public Vector3 Origintrans; // 発射元の座標
    public Vector3 shootForward;
    public BulletManager.ShootChara shootChara; // 誰が発射したか
    public BulletSetting.BulletList bulletType; // 弾の種類
    // パラメータの設定
    public BulletData(Vector3 pos, Vector3 rot, BulletManager.ShootChara chara, BulletSetting.BulletList list)
    {
        Origintrans = pos;
        shootForward = rot;
        shootChara = chara;

        bulletType = list;
        switch (chara)
        {
            case BulletManager.ShootChara.Player:
                bulletSpeed = 30.0f;
                break;
            case BulletManager.ShootChara.Enemy:
                bulletSpeed = 20.0f;
                break;
        }
        // 生成予定データリストにこのデータを追加
        BulletSpawner.Instance.bulletDataList.Add(this);
    }
}
