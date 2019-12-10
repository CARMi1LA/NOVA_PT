using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Player
{
    public int MAX_LV = 3;
    // プレイヤーのパラメータごとのレベル
    public IntReactiveProperty lv_HP = new IntReactiveProperty(0);            // HP
    public IntReactiveProperty lv_Spd = new IntReactiveProperty(0);         // スピード
    public IntReactiveProperty lv_Int = new IntReactiveProperty(0);    // 攻撃間隔

    public LevelData_Player()
    {
        lv_HP.Value = 0;
        lv_Spd.Value = 0;
        lv_Int.Value = 0;
    }

}
