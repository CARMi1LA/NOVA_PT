using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDBulletData
{
    // Bullet生成に必要な情報

    public enum BulletParentList
    {
        Player = 0,
        Enemy  = 1
    }
    public BulletParentList bParent = BulletParentList.Player;

    public enum BulletTypeList
    {
        Normal  = 0,
        Missile = 1,
        Bomb    = 2,
        Boost   = 3
    }
    public BulletTypeList bType = BulletTypeList.Normal;

    public Vector3 bPosition;           // 弾の初期位置
    public Vector3 bRotation;           // 弾の初期回転

    // 全員ほぼ同一データなのでScriptableObjectにひとまとめにしようかな
    public float bSpeed = 250.0f;                // 弾のスピード
    public float bDeathCount = 2.0f;    // 自動消滅までの時間 (単位：秒)
    public int bDamage = 1;             // 弾のダメージ量 (基本的に1)

    // コンストラクタ
    public TDBulletData(BulletParentList parent, BulletTypeList type,Vector3 position ,Vector3 euler)
    {
        bParent = parent;
        bType   = type;
        bPosition = position;
        bRotation = euler;
    }
}
