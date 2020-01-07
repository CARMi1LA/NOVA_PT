using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDEnemyWaveList : MonoBehaviour
{
    public List<TDEnemyWave> enemyWaveList;

    public TDEnemyWave GetEnemyWave(int waveCount)
    {
        return enemyWaveList[waveCount];
    }
}

[System.Serializable]
public class TDEnemyWave
{

}
