using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStatus : MonoBehaviour
{
    public enum ShootByChara
    {
        None = 0,
        Player = 1,
        Enemy = 2
    }

    public float bulletSpeed;     // 速度
    public float[] bulletAngle;     // 弾の発射角度
    public Transform Origintrans; // 発射元の座標

    public ShootByChara shootChara; // 誰が発射したか
    public Color bulletColor;                   // 弾の色情報
}
