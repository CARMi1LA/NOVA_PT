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
    public IntReactiveProperty mater = new IntReactiveProperty(0);                   // 所持マター

    private ShopDataList shopDataList;      // 読み込むショップのデータリスト
    public ShopData shopData;               // 読み込まれたショップのデータ
    public SpBtnPlayerManager btnPlayer;
    public ShopBuyController spBuyControll;

    public Subject<ShopData.Player_ParamList> addLevel_Player = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.TowerRed_ParamList> addLevel_TowerRed = new Subject<ShopData.TowerRed_ParamList>();
    public Subject<ShopData.TowerBlue_ParamList> addLevel_TowerBlue = new Subject<ShopData.TowerBlue_ParamList>();
    public Subject<ShopData.TowerYellow_ParamList> addLevel_TowerYellow = new Subject<ShopData.TowerYellow_ParamList>();
    public Subject<ShopData.TowerGreen_ParamList> addLevel_TowerGreen = new Subject<ShopData.TowerGreen_ParamList>();
    public Subject<ShopData.Skill_ParamList> addLevel_Skill = new Subject<ShopData.Skill_ParamList>();
    public Subject<ShopData.Ult_ParamList> addLevel_Ult = new Subject<ShopData.Ult_ParamList>();
    public Subject<Unit> InitParamLevel = new Subject<Unit>();
    protected override void Awake()
    {
        base.Awake();
        shopDataList = Resources.Load<ShopDataList>("ShopDataList");
        shopData = shopDataList.dataList_Shop[0];

    }

    // Start is called before the first frame update
    void Start()
    {
        InitParamLevel.Subscribe(_ => 
        {
            shopData.levelData_Player = new LevelData_Player();
        }).AddTo(this.gameObject);

        InitParamLevel.OnNext(Unit.Default);

        spBuyControll.BuyControllerInit.OnNext(shopData);
        addLevel_Player
            .Subscribe(val => 
            {
                switch (val)
                {
                    case ShopData.Player_ParamList.Param_HP:
                        mater.Value -= shopData.shopData_Player[shopData.levelData_Player.level_HP.Value + 1].purchaseMater;
                        shopData.levelData_Player.level_HP.Value++;
                        btnPlayer.ChangeLvText.OnNext(ShopData.Player_ParamList.Param_HP);
                        btnPlayer.ChangeValueText.OnNext(ShopData.Player_ParamList.Param_HP);
                        break;
                    case ShopData.Player_ParamList.Param_Speed:
                        mater.Value -= shopData.shopData_Player[shopData.levelData_Player.level_Speed.Value + 1].purchaseMater;
                        shopData.levelData_Player.level_Speed.Value++;
                        btnPlayer.ChangeLvText.OnNext(ShopData.Player_ParamList.Param_Speed);
                        btnPlayer.ChangeValueText.OnNext(ShopData.Player_ParamList.Param_Speed);
                        break;
                    case ShopData.Player_ParamList.Param_Interval:
                        mater.Value -= shopData.shopData_Player[shopData.levelData_Player.level_Interval.Value + 1].purchaseMater;
                        shopData.levelData_Player.level_Interval.Value++;
                        btnPlayer.ChangeLvText.OnNext(ShopData.Player_ParamList.Param_Interval);
                        btnPlayer.ChangeValueText.OnNext(ShopData.Player_ParamList.Param_Interval);
                        break;
                }
            }).AddTo(this.gameObject);

        addLevel_TowerRed
            .Subscribe(val => 
            {

            }).AddTo(this.gameObject);

        addLevel_Skill
            .Where(val => mater.Value >= shopData.shopData_Skill[(int)val].purchaseMater)
            .Subscribe(val =>
            {
                if ((int)val == shopData.levelData_Skill.level_Skill.Value)
                {

                }
                else
                {
                    shopData.levelData_Skill.level_Skill.Value = (int)val;
                }
            }).AddTo(this.gameObject);

        addLevel_Ult
            .Where(val => mater.Value >= shopData.shopData_Ult[(int)val].purchaseMater)
            .Subscribe(val =>
            {
                if ((int)val == shopData.levelData_Ult.level_Ult.Value)
                {

                }
                else
                {
                    shopData.levelData_Ult.level_Ult.Value = (int)val;
                }
            }).AddTo(this.gameObject);
    }

    [EnumAction(typeof(ShopData.Player_ParamList))]
    public void OnClickPlayerParam(int player_Param)
    {
        addLevel_Player.OnNext((ShopData.Player_ParamList)player_Param);
    }

    [EnumAction(typeof(ShopData.TowerRed_ParamList))]
    public void OnClickTowerRedParam(int towerRed_Param)
    {
        addLevel_TowerRed.OnNext((ShopData.TowerRed_ParamList)towerRed_Param);
    }

    [EnumAction(typeof(ShopData.TowerBlue_ParamList))]
    public void OnClickTowerBlueParam(int towerBlue_Param)
    {
        addLevel_TowerBlue.OnNext((ShopData.TowerBlue_ParamList)towerBlue_Param);
    }

    [EnumAction(typeof(ShopData.TowerYellow_ParamList))]
    public void OnClickTowerYellowParam(int towerYellow_Param)
    {
        addLevel_TowerYellow.OnNext((ShopData.TowerYellow_ParamList)towerYellow_Param);
    }

    [EnumAction(typeof(ShopData.TowerYellow_ParamList))]
    public void OnClickTowerGreenParam(int towerGreen_Param)
    {
        addLevel_TowerGreen.OnNext((ShopData.TowerGreen_ParamList)towerGreen_Param);
    }

    [EnumAction(typeof(ShopData.Skill_ParamList))]
    public void OnClickSkillParam(int skill_ParamList)
    {
        addLevel_Skill.OnNext((ShopData.Skill_ParamList)skill_ParamList);
    }

    [EnumAction(typeof(ShopData.Ult_ParamList))]
    public void OnClickUltParam(int ult_ParamList)
    {
        addLevel_Ult.OnNext((ShopData.Ult_ParamList)ult_ParamList);
    }
}
