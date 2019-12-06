using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Ult
{
    // Ultのレベルデータ
    // （正確にはレベルの概念は無いが便宜上表記）
    public LevelData_Ult()
    {
        level_Ult = new IntReactiveProperty(0);
    }

    public IntReactiveProperty level_Ult;           // 必殺技番号

}
