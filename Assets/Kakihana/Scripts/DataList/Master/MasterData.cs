using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MasterData 
{
    // ゲームに必要なデータ
    // 難易度
    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    // タワーの色
    public enum TowerColor
    {
        Red = 0,
        Blue = 1,
        Yellow = 2,
        Green = 3
    }

    public Difficulty difficulty;

    // 待機時間
    public int waitTime;
    // 各戦闘ウェーブの時間
    public int[] waveTime;

    // 敵最大出現数
    public int enemyCoreMax;
    // アイテム最大出現数
    public int itemDropMax;
}
