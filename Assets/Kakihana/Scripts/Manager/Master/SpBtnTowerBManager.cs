using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnTowerBManager : MonoBehaviour
{
    private LevelData_Tower blueTower_Lv;
    public ShopBtnManager[] spPlayerBtn;

    // 初期化イベント
    public Subject<LevelData_Player> InitSubject = new Subject<LevelData_Player>();
    // レベル更新イベント
    public Subject<ShopData.Player_ParamList> ChangeLvText = new Subject<ShopData.Player_ParamList>();
    // 必要金額更新イベント
    public Subject<ShopData.Player_ParamList> ChangeValueText = new Subject<ShopData.Player_ParamList>();
    // 購入可能時のイベント
    public Subject<ShopData.Player_ParamList> BuyOkText = new Subject<ShopData.Player_ParamList>();
    // 購入不可能時のイベント
    public Subject<ShopData.Player_ParamList> BuyNgText = new Subject<ShopData.Player_ParamList>();
    // レベル最大時のイベント
    public Subject<ShopData.Player_ParamList> SoldOutText = new Subject<ShopData.Player_ParamList>();
    // 次レベル設定イベント
    public Subject<ShopData.Player_ParamList> NextLv = new Subject<ShopData.Player_ParamList>();
    // Start is called before the first frame update
    void Start()
    {
        blueTower_Lv = ShopManager.Instance.shopData.levelData_Tower[(int)ShopData.TowerColor.Blue];
    }
}
