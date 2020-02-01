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
        lvHPText.text = "";
        for(int i = 0; i < lvData.playerLv.lv_HP.Value; ++i)
        {
            lvHPText.text += "O";
        }
        lvHPText.text += "   HP";

        lvAtkText.text = "";
        for (int i = 0; i < lvData.playerLv.lv_Int.Value; ++i)
        {
            lvAtkText.text += "O";
        }
        lvAtkText.text += " ATK";

        lvSpdText.text = "";
        for (int i = 0; i < lvData.playerLv.lv_Spd.Value; ++i)
        {
            lvSpdText.text += "O";
        }
        lvSpdText.text += " SPD";

        // SkillとUltimateの表示
        switch (lvData.skillLv.level_Skill.Value)
        {
            case 0:
                skillText.text = "W Slicer：S";
                break;
            case 1:
                skillText.text = "Razer：S";
                break;
            case 2:
                skillText.text = "Missile：S";
                break;
            case 3:
                skillText.text = "Power Bash：S";
                break;
            default:
                skillText.text = "Error：S";
                break;
        }
        switch (lvData.ultLv.level_Ult.Value)
        {
            case 0:
                ultimateText.text = "Barrier：U";
                break;
            case 1:
                ultimateText.text = "Slow Trap：U";
                break;
            case 2:
                ultimateText.text = "Sword Field：U";
                break;
            case 3:
                ultimateText.text = "Repair：U";
                break;
            default:
                ultimateText.text = "Error：U";
                break;
        }
    }
}
