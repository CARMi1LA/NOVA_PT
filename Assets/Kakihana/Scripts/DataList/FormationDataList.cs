using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "EnemyFormList",
  menuName = "ScriptableObject/FormationDataList",
  order = 1)
]
public class FormationDataList : ScriptableObject
{
    public List<EnemyFormationData> formDataList = new List<EnemyFormationData>();
}
