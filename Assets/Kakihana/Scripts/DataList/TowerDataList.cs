using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 右クリックでエディター上からリストを作成できるようにする
// 拠点のデータリスト
[CreateAssetMenu(
  fileName = "TowerDataList",
  menuName = "ScriptableObject/TowerDataList",
  order = 3)
]
public class TowerDataList : ScriptableObject
{
    public List<TowerData> EnemyStatusList = new List<TowerData>();
}
