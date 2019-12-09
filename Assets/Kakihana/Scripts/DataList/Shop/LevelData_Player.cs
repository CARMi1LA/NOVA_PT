using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Player
{
    public LevelData_Player()
    {
        level_HP = new IntReactiveProperty(0);
        level_Speed = new IntReactiveProperty(0);
        level_Interval = new IntReactiveProperty(0);
    }

    // プレイヤーのパラメータごとのレベル
    public IntReactiveProperty level_HP;            // HP
    public IntReactiveProperty level_Speed;         // スピード
    public IntReactiveProperty level_Interval;      // 攻撃間隔
}
