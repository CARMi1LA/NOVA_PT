using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    // ステージ情報格納クラス

    public enum StageDifficuty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    public StageDifficuty stageDifficuty;   // ステージ難易度
    public int waveNum;                     // ウェーブ数
    public float[] attackPhaseTime;         // 攻撃フェーズの時間
    public float[] intervalTime;            // 次のフェーズまでの待機時間

    public List<ValueList> spawnEnemyObj = new List<ValueList>();

}

//Inspectorに複数データを表示するためのクラス
[System.SerializableAttribute]
public class ValueList
{
    public List<EnemyUnitManager> List = new List<EnemyUnitManager>();

    public ValueList(List<EnemyUnitManager> list)
    {
        List = list;
    }
}
