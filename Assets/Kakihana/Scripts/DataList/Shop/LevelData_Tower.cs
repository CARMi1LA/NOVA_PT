using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Tower
{
    public IntReactiveProperty level_Trap;      // トラップ
    public IntReactiveProperty level_Turret;    // タレット
    public IntReactiveProperty level_Tower;     // タワー本体
    public IntReactiveProperty level_Repair;    // 修理回数
}
