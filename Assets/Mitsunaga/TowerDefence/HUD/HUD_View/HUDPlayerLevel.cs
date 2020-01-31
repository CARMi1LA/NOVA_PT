using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDPlayerLevel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lvHPText;
    [SerializeField] TextMeshProUGUI lvAtkText;
    [SerializeField] TextMeshProUGUI lvSpdText;
    [SerializeField] TextMeshProUGUI skillText;
    [SerializeField] TextMeshProUGUI ultimateText;

    public void SetLevels(SpLvData lvData)
    {
        // 各Levelの表示
        lvHPText.text = "HP   ";
        for(int i = 0; i < lvData.playerLv.lv_HP.Value; ++i)
        {
            lvHPText.text += "O";
        }
        lvAtkText.text = "Atk  ";
        for (int i = 0; i < lvData.playerLv.lv_Int.Value; ++i)
        {
            lvAtkText.text += "O";
        }
        lvSpdText.text = "Spd  ";
        for (int i = 0; i < lvData.playerLv.lv_Spd.Value; ++i)
        {
            lvSpdText.text += "O";
        }
        // SkillとUltimateの表示
        switch (lvData.skillLv.level_Skill.Value)
        {
            case 0:
                skillText.text = "Skill：W Slicer";
                break;
            case 1:
                skillText.text = "Skill：Razer";
                break;
            case 2:
                skillText.text = "Skill：Missile";
                break;
            case 3:
                skillText.text = "Skill：Bomb";
                break;
            default:
                skillText.text = "Skill：Error";
                break;
        }
        switch (lvData.ultLv.level_Ult.Value)
        {
            case 0:
                ultimateText.text = "Ult  ：Barrier";
                break;
            case 1:
                ultimateText.text = "Ult  ：Slow Trap";
                break;
            case 2:
                ultimateText.text = "Ult  ：Big Bomb";
                break;
            case 3:
                ultimateText.text = "Ult  ：Repair";
                break;
            default:
                ultimateText.text = "Ult  ：Error";
                break;
        }
    }
}
