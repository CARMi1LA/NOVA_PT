using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDTowerLevel : MonoBehaviour
{
    // タワー関連のレベルを表示

    [SerializeField]
    List<TextMeshProUGUI> systemTextList = new List<TextMeshProUGUI>();
    [SerializeField]
    List<TextMeshProUGUI> turretTextList = new List<TextMeshProUGUI>();
    [SerializeField]
    List<TextMeshProUGUI> trapTextList   = new List<TextMeshProUGUI>();

    public void SetLevels(SpLvData lvData)
    {
        for(int tCount = 0; tCount < systemTextList.Count; ++tCount)
        {
            systemTextList[tCount].text = "System ";
            for (int count = 0; count < lvData.towerLv[tCount].level_Tower.Value; ++count)
            {
                systemTextList[tCount].text += "O";
            }
            turretTextList[tCount].text = "Turret   ";
            for (int count = 0; count < lvData.towerLv[tCount].level_Turret.Value; ++count)
            {
                turretTextList[tCount].text += "O";
            }
            trapTextList[tCount].text = "Trap      ";
            for (int count = 0; count < lvData.towerLv[tCount].level_Trap.Value; ++count)
            {
                trapTextList[tCount].text += "O";
            }
        }
    }
}
