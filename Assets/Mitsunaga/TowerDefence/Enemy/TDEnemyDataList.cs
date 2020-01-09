using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TDEnemyDataList", menuName = "TDScriptable/TDEnemyDataList", order = 2)]
public class TDEnemyDataList : ScriptableObject
{
    public List<TDEnemyData> enemyDataList;

    // エネミーのサイズと型からそれに合ったエネミーデータを取得する
    public TDEnemyData GetEnemyData(TDList.EnemySizeList size,TDList.EnemyTypeList type)
    {
        TDEnemyData eData = null;

        foreach(var data in enemyDataList)
        {
            if (size == data.eSize && type == data.eType)
            {
                eData = data;
            }
        }
        return eData;
    }
}

[System.Serializable]
public class TDEnemyData
{
    public TDList.EnemySizeList eSize;  // S,M,L,XL
    public TDList.EnemyTypeList eType;  // Attack,Defence,Speed

    public int eHealth;
    public int eCoreHealth;

    public float eSpeed;
    public float eDashSpeed;

    public int eDropMater;
}
