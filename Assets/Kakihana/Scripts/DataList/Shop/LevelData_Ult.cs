using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Ult
{
    // Ult＆スキルのレベルデータ
    // （正確にはレベルの概念は無いが便宜上表記）

    public IntReactiveProperty level_Skill;         // スキル番号
    public IntReactiveProperty level_Ult;           // 必殺技番号

}
