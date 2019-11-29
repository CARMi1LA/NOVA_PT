using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopPlayerPramData
{
    public int maxHP;               // 設定する最大HP
    public float speedMul;          // プレイヤーの速度倍率
    public float shootIntervalMul;  // 発射間隔の倍率
    public int purchaseMater;       // 必要金額
}
