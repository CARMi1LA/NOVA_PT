using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Player
{
    // プレイヤーのパラメータごとのレベル
    public IntReactiveProperty level_HP;            // HP
    public IntReactiveProperty level_Speed;         // スピード
    public IntReactiveProperty level_Interval;      // 攻撃間隔
}
