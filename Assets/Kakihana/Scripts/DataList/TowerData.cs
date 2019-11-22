using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    // 拠点データ

    public int towerHP;                     // 拠点の耐久力
    public float emergencyRange;            // 自動保護システムを使用する距離
    public int apsInterval;                 // 自動保護システム（APS）のリキャスト時間

    public TrapManager trapManager;         // トラップ関連のコンポーネント
}
