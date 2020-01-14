using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MasterData 
{
    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    public enum TowerColor
    {
        Red = 0,
        Blue = 1,
        Yellow = 2,
        Green = 3
    }

    public Difficulty difficulty;

    public int waitTime;
    public int[] waveTime;
}
