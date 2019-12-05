using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnTowerRManager : MonoBehaviour
{
    private LevelData_Tower redTower_Lv;
    public ShopBtnManager[] spPlayerBtn;
    // Start is called before the first frame update
    void Start()
    {
        redTower_Lv = ShopManager.Instance.shopData.levelData_Tower[(int)ShopData.TowerColor.Red];

    }
}
