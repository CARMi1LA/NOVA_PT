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
    public float rDelay;        // レーザー発射時の待機時間
    public float rRadius;       // レーザーの太さ
    public Vector3 rPosition;   // レーザーの始点
    public Vector3 rRotation;   // レーザーの方向

    // コンストラクタ
    public RazerData(RazerParent rp,float delay,float radius,Vector3 position,Vector3 euler)
    {
        rParent = rp;
        rDelay = delay;
        rRadius = radius;
        rPosition = position;
        rRotation = euler;
    }
}
