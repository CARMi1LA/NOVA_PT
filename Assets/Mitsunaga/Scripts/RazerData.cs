using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerData : MonoBehaviour
{
    // レーザー生成素材

    public enum RazerParent
    {
        Null = 0,
        Player = 1,
        Enemy = 2,
        Boss = 3
    }
    public RazerParent rParent; // 生成元
    public float rDelay;        // 発射時の待機時間
    public Vector3 rPosition;   // 発射の始点
    public Vector3 rRotation;   // 発射の方向

    // コンストラクタ
    public RazerData(RazerParent rp,float delay,Vector3 position,Vector3 euler)
    {
        rParent = rp;
        rDelay = delay;
        rPosition = position;
        rRotation = euler;
    }
}
