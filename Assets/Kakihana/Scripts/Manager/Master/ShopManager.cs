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
    public Subject<ShopData.Ult_ParamList> addLevel_Ult = new Subject<ShopData.Ult_ParamList>();

    protected override void Awake()
    {
        base.Awake();
        shopDataList = Resources.Load<ShopDataList>("ShopDataList");
        shopData = shopDataList.dataList_Shop[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        addLevel_Player
            .Where(val => mater >= shopData.shopData_Player[(int)val].purchaseMater)
            .Subscribe(val => 
            {
                switch (val)
                {
                    case ShopData.Player_ParamList.Param_HP:
                        shopData.levelData_Player.level_HP.Value++;
                        break;
                    case ShopData.Player_ParamList.Param_Speed:
                        shopData.levelData_Player.level_Speed.Value++;
                        break;
                    case ShopData.Player_ParamList.Param_Interval:
                        shopData.levelData_Player.level_Interval.Value++;
                        break;
                }
            }).AddTo(this.gameObject);

        addLevel_TowerRed
            .Subscribe(val => { }).AddTo(this.gameObject);

        addLevel_Skill
            .Where(val => mater >= shopData.shopData_Skill[(int)val].purchaseMater)
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
            .Where(val => mater >= shopData.shopData_Ult[(int)val].purchaseMater)
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

    public void OnClickPlayerParam(ShopData.Player_ParamList player_Param)
    {
        addLevel_Player.OnNext(player_Param);
    }

    public void OnClickTowerParam(ShopData.TowerRed_ParamList towerRed_Param)
    {
        addLevel_TowerRed.OnNext(towerRed_Param);
    }

    public void OnClickTowerParam(ShopData.TowerBlue_ParamList towerBlue_Param)
    {
        addLevel_TowerBlue.OnNext(towerBlue_Param);
    }

    public void OnClickTowerParam(ShopData.TowerYellow_ParamList towerYellow_Param)
    {
        addLevel_TowerYellow.OnNext(towerYellow_Param);
    }

    public void OnClickTowerParam(ShopData.TowerGreen_ParamList towerGreen_Param)
    {
        addLevel_TowerGreen.OnNext(towerGreen_Param);
    }

    public void OnClickSkillParam(ShopData.Skill_ParamList skill_ParamList)
    {
        addLevel_Skill.OnNext(skill_ParamList);
    }

    public void OnClickUltParam(ShopData.Ult_ParamList ult_ParamList)
    {
        addLevel_Ult.OnNext(ult_ParamList);
    }
}
