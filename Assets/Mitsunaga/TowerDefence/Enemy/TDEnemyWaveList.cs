using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// １つのウェーブ
[CreateAssetMenu(fileName ="TDEnemyWaveList", menuName = "TDScriptable/TDEnemyWaveList", order = 3)]
public class TDEnemyWaveList : ScriptableObject
{
    public float enemyWaveInterval = 5.0f;
    public List<TDEnemyWave> enemyWave;
}

// １つの間隔
[System.Serializable]
public class TDEnemyWave
{
    public List<TDList.EnemySizeList> enemyWavePart;
}
