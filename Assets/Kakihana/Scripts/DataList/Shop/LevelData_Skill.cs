using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class LevelData_Skill
{
    // スキルのレベルデータ
    // （正確にはレベルの概念は無いが便宜上表記）

    public LevelData_Skill()
    {
        level_Skill = new IntReactiveProperty(0);
    }

    public IntReactiveProperty level_Skill;         // スキル番号
}
