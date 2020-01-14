using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "MasterDataList",
  menuName = "ScriptableObject/MasterDataList",
  order = 4)
]
public class MasterDataList : ScriptableObject
{
    public List<MasterData> masterDataList = new List<MasterData>();
}
