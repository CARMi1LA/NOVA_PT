using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnTowerBManager : MonoBehaviour
{
    private LevelData_Tower blueTower_Lv;
    public ShopBtnManager[] spPlayerBtn;
    // Start is called before the first frame update
    void Start()
    {
        blueTower_Lv = ShopManager.Instance.shopData.levelData_Tower[(int)ShopData.TowerColor.Blue];
    }
}
