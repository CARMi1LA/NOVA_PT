using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ShopManager : SPMSinleton<ShopManager>
{
    // ショップクラス
    // 
    [SerializeField]
    const int LEVEL_ARRAYSIZE = 3;          // 最大レベル
    public int mater = 0;                   // 所持マター

    private ShopDataList shopDataList;      // 読み込むショップのデータリスト
    public ShopData shopData;               // 読み込まれたショップのデータ

    public Subject<ShopData.Player_ParamList> addLevel_Player = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.TowerRed_ParamList> addLevel_TowerRed = new Subject<ShopData.TowerRed_ParamList>();
    public Subject<ShopData.TowerBlue_ParamList> addLevel_TowerBlue = new Subject<ShopData.TowerBlue_ParamList>();
    public Subject<ShopData.TowerYellow_ParamList> addLevel_TowerYellow = new Subject<ShopData.TowerYellow_ParamList>();
    public Subject<ShopData.TowerGreen_ParamList> addLevel_TowerGreen = new Subject<ShopData.TowerGreen_ParamList>();
    public Subject<ShopData.Skill_ParamList> addLevel_Skill = new Subject<ShopData.Skill_ParamList>();
    public Subject<ShopData.Player_ParamList> addLevel_Ult = new Subject<ShopData.Player_ParamList>();

    protected override void Awake()
    {
        base.Awake();
        shopDataList = Resources.Load<ShopDataList>("ShopDataList");
        shopData = shopDataList.dataList_Shop[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        addLevel_Player.Subscribe(_ => 
        {

        }).AddTo(this.gameObject);
    }

    public void OnClickPlayerParam(ShopData.Player_ParamList player_Param)
    {

    }

    public void OnClickTowerParam(ShopData.TowerRed_ParamList towerRed_Param)
    {

    }

    public void OnClickTowerParam(ShopData.TowerBlue_ParamList towerBlue_Param)
    {

    }

    public void OnClickTowerParam(ShopData.TowerYellow_ParamList towerYellow_Param)
    {

    }

    public void OnClickTowerParam(ShopData.TowerGreen_ParamList towerGreen_Param)
    {

    }

    public void OnClickSkillParam()
    {

    }

    public void OnClickUltParam()
    {

    }
}
