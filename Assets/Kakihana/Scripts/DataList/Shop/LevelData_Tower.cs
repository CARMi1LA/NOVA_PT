using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Tower
{
    public LevelData_Tower()
    {
        level_Trap = new IntReactiveProperty(0);
        level_Turret = new IntReactiveProperty(0);
        level_Tower = new IntReactiveProperty(0);
        level_Repair = new IntReactiveProperty(0);
    }

    public int MAX_LV = 3;
    public IntReactiveProperty level_Trap;      // トラップ
    public IntReactiveProperty level_Turret;    // タレット
    public IntReactiveProperty level_Tower;     // タワー本体
    public IntReactiveProperty level_Repair;    // 修理回数
}
