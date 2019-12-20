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
    public TDList.EnemySizeList eSize;
    public TDList.EnemyTypeList eType;

    public int eHealth;
    public int eCoreHealth;

    public float eSpeed = 100.0f;
    public float eDashSpeed = 300.0f;

    public int eDropMater = 50;
}
