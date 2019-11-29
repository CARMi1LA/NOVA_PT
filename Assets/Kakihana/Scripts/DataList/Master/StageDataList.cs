using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "StageDataList",
  menuName = "ScriptableObject/StageDataList",
  order = 2)
]
public class StageDataList : ScriptableObject
{
    public List<StageData> stageDataList = new List<StageData>();
}
