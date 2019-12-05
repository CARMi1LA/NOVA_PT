using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnTowerYManager : MonoBehaviour
{
    private LevelData_Tower yellowTower_Lv;
    public ShopBtnManager[] spPlayerBtn;
    // Start is called before the first frame update
    void Start()
    {
        yellowTower_Lv = ShopManager.Instance.shopData.levelData_Tower[(int)ShopData.TowerColor.Yellow];
    }
}
