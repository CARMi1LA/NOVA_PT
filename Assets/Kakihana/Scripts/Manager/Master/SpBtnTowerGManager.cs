using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnTowerGManager : MonoBehaviour
{
    private LevelData_Tower greenTower_Lv;
    public ShopBtnManager[] spPlayerBtn;
    // Start is called before the first frame update
    void Start()
    {
        greenTower_Lv = ShopManager.Instance.shopData.levelData_Tower[(int)ShopData.TowerColor.Green];
    }
}
