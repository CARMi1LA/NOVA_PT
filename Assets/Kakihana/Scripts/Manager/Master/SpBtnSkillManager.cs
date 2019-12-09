using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpBtnSkillManager : MonoBehaviour
{
    private LevelData_Skill skillData_Lv;
    public ShopBtnManager[] spSKillBtn;
    // Start is called before the first frame update
    void Start()
    {
        skillData_Lv = ShopManager.Instance.shopData.levelData_Skill;
    }
}
