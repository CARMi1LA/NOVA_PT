using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Tower
{
    // タワーのパラメータごとのレベル
    public enum TowerColor
    {
        Red = 0,
        Blue,
        Yellow,
        Green
    }
    public TowerColor towerColor;               // タワーの色
    public IntReactiveProperty level_Trap;      // トラップ
    public IntReactiveProperty level_Turret;    // タレット
    public IntReactiveProperty level_Tower;     // タワー本体
    public IntReactiveProperty level_Repair;    // 修理回数
}
