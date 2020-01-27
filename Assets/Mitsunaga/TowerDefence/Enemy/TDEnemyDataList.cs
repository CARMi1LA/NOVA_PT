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

        foreach(var item in enemyDataList)
        {
            if (size == item.eSize && type == item.eType)
            {
                eData = item;
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
    public float eSpeedMul;

    public float eRotSpeed;

    public int eTowerDamage;

    public int eDropMater;
}
