using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnUltManager : MonoBehaviour
{
    private LevelData_Ult ultData_Lv;
    public ShopBtnManager[] spUltBtn;
    // Start is called before the first frame update
    void Start()
    {
        ultData_Lv = ShopManager.Instance.shopData.levelData_Ult;
    }
}
