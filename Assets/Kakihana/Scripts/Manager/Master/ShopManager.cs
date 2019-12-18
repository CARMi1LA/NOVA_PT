using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ShopManager : SPMSinleton<ShopManager>
{
    // ショップクラス
    /*
     * 【挙動】
     * ・各ショップ関連のクラスの初期化
     * ・購入に必要な通貨（マター）の保存
     * ・ショップボタンが押されたときのイベントを管理
     * ・各パラメータレベルの管理
     * ・イベントに応じて、他クラスに特定の処理を命令させる
    */ 

    // 所持マター
    public IntReactiveProperty mater = new IntReactiveProperty(0);                   // 所持マター

    private ShopDataList shopDataList;      // 読み込むショップのデータリスト
    public ShopData shopData;               // 読み込まれたショップのデータ
    public SpBtnPlayerManager btnPlayer;    // プレイヤー購入画面のボタン管理クラス
    public SpBtnTowerRManager btnTwR;
    public SpBtnTowerBManager btnTwB;
    public SpBtnTowerYManager btnTwY;
    public SpBtnTowerGManager btnTwG;
    public SpBtnSkillManager btnSkill;
    public SpBtnUltManager btnUlt;
    public ShopBuyController spBuyControll; // マターを監視し、適切なUIを表示させるクラス
    public SpLvData spLv;                   // 各ショップ運営に必要なレベルクラス

    // プレイヤー購入画面のボタンが押されたときのイベント
    public Subject<ShopData.Player_ParamList> addLevel_Player = new Subject<ShopData.Player_ParamList>();
    // タワー（赤）購入画面のボタンが押されたときのイベント
    public Subject<ShopData.TowerRed_ParamList> addLevel_TowerRed = new Subject<ShopData.TowerRed_ParamList>();
    // タワー（青）購入画面のボタンが押されたときのイベント
    public Subject<ShopData.TowerBlue_ParamList> addLevel_TowerBlue = new Subject<ShopData.TowerBlue_ParamList>();
    // タワー（黄）購入画面のボタンが押されたときのイベント
    public Subject<ShopData.TowerYellow_ParamList> addLevel_TowerYellow = new Subject<ShopData.TowerYellow_ParamList>();
    // タワー（緑）購入画面のボタンが押されたときのイベント
    public Subject<ShopData.TowerGreen_ParamList> addLevel_TowerGreen = new Subject<ShopData.TowerGreen_ParamList>();
    // スキル購入画面のボタンが押されたときのイベント
    public Subject<ShopData.Skill_ParamList> addLevel_Skill = new Subject<ShopData.Skill_ParamList>();
    // 必殺技購入画面のボタンが押されたときのイベント
    public Subject<ShopData.Ult_ParamList> addLevel_Ult = new Subject<ShopData.Ult_ParamList>();
    // 初期化イベント
    public Subject<Unit> InitParamLevel = new Subject<Unit>();
    protected override void Awake()
    {
        base.Awake();
        // 読み込みたいショップデータリストをここで設定
        shopDataList = Resources.Load<ShopDataList>("ShopDataList");
        // 実際に使うデータをインデックスより設定
        shopData = shopDataList.dataList_Shop[0];
        // 各パラメータレベルの初期化
        spLv.SpLvInit.OnNext(Unit.Default);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        InitParamLevel.Subscribe(_ =>
        {
            // マター監視クラスの初期化
            spBuyControll.BuyInit();
            btnSkill.normal.Value = true;
            btnUlt.normal.Value = true;
        }).AddTo(this.gameObject);

        // 起動時に初期化を行う
        InitParamLevel.OnNext(Unit.Default);

        // プレイヤー購入画面のボタンが押されたときのイベント
        addLevel_Player
            .Subscribe(val => 
            {
                switch (val)
                {
                    // HP強化ボタンが押された
                    case ShopData.Player_ParamList.Param_HP:
                        // 所持マターを購入金額ぶん除算
                        mater.Value -= shopData.shopData_Player[spLv.playerLv.lv_HP.Value + 1].purchaseMater;
                        // パラメータレベルを１上げる
                        spLv.playerLv.lv_HP.Value++;
                        // 最大レベルであれば購入できないようにする
                        if (spLv.playerLv.lv_HP.Value >= spLv.playerLv.MAX_LV)
                        {
                            // 最大レベル時の処理
                            btnPlayer.SoldOutText.OnNext(ShopData.Player_ParamList.Param_HP);
                        }
                        else
                        {
                            // 通常処理
                            btnPlayer.NextLv.OnNext(ShopData.Player_ParamList.Param_HP);
                            btnPlayer.ChangeLvText.OnNext(ShopData.Player_ParamList.Param_HP);
                            btnPlayer.ChangeValueText.OnNext(ShopData.Player_ParamList.Param_HP);
                        }
                        break;
                    // スピード強化ボタンが押された
                    case ShopData.Player_ParamList.Param_Speed:
                        mater.Value -= shopData.shopData_Player[spLv.playerLv.lv_Spd.Value + 1].purchaseMater;
                        spLv.playerLv.lv_Spd.Value++;
                        if (spLv.playerLv.lv_Spd.Value >= spLv.playerLv.MAX_LV)
                        {
                            btnPlayer.SoldOutText.OnNext(ShopData.Player_ParamList.Param_Speed);
                        }
                        else
                        {
                            btnPlayer.NextLv.OnNext(ShopData.Player_ParamList.Param_Speed);
                            btnPlayer.ChangeLvText.OnNext(ShopData.Player_ParamList.Param_Speed);
                            btnPlayer.ChangeValueText.OnNext(ShopData.Player_ParamList.Param_Speed);
                        }
                        break;
                    // 攻撃間隔強化ボタンが押された
                    case ShopData.Player_ParamList.Param_Interval:
                        mater.Value -= shopData.shopData_Player[spLv.playerLv.lv_Int.Value + 1].purchaseMater;
                        spLv.playerLv.lv_Int.Value++;
                        if (spLv.playerLv.lv_Int.Value >= spLv.playerLv.MAX_LV)
                        {
                            btnPlayer.SoldOutText.OnNext(ShopData.Player_ParamList.Param_Interval);
                        }
                        else
                        {
                            btnPlayer.NextLv.OnNext(ShopData.Player_ParamList.Param_Interval);
                            btnPlayer.ChangeLvText.OnNext(ShopData.Player_ParamList.Param_Interval);
                            btnPlayer.ChangeValueText.OnNext(ShopData.Player_ParamList.Param_Interval);
                        }
                        break;
                }
            }).AddTo(this.gameObject);

        // 赤タワー購入ボタンプッシュイベント //
        addLevel_TowerRed
            .Subscribe(val => 
            {
                switch (val)
                {
                    case ShopData.TowerRed_ParamList.Param_Trap:
                        mater.Value -= shopData.redData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value >= spLv.towerLv[(int)ShopData.TowerColor.Red].MAX_LV)
                        {
                            btnTwR.SoldOutText.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
                        }
                        else
                        {
                            btnTwR.NextLv.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
                            btnTwR.ChangeLvText.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
                            btnTwR.ChangeValueText.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
                        }
                        break;
                    case ShopData.TowerRed_ParamList.Param_Turret:
                        mater.Value -= shopData.redData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value >= spLv.towerLv[(int)ShopData.TowerColor.Red].MAX_LV)
                        {
                            btnTwR.SoldOutText.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
                        }
                        else
                        {
                            btnTwR.NextLv.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
                            btnTwR.ChangeLvText.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
                            btnTwR.ChangeValueText.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
                        }
                        break;
                    case ShopData.TowerRed_ParamList.Param_Tower:
                        mater.Value -= shopData.redData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value >= spLv.towerLv[(int)ShopData.TowerColor.Red].MAX_LV)
                        {
                            btnTwR.SoldOutText.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
                        }
                        else
                        {
                            btnTwR.NextLv.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
                            btnTwR.ChangeLvText.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
                            btnTwR.ChangeValueText.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
                        }
                        break;
                    case ShopData.TowerRed_ParamList.Repair:
                        mater.Value -= (shopData.redData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Red].level_Repair.Value + 1].purchaseMater * 5);
                        spLv.towerLv[(int)ShopData.TowerColor.Red].level_Repair.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Red].level_Repair.Value >= spLv.MAX_LEVEL)
                        {
                            btnTwR.SoldOutText.OnNext(ShopData.TowerRed_ParamList.Repair);
                        }
                        else
                        {
                            btnTwR.NextLv.OnNext(ShopData.TowerRed_ParamList.Repair);
                            btnTwR.ChangeLvText.OnNext(ShopData.TowerRed_ParamList.Repair);
                            btnTwR.ChangeValueText.OnNext(ShopData.TowerRed_ParamList.Repair);
                        }
                        break;
                    default:
                        break;
                }
            }).AddTo(this.gameObject);

        // 青タワー購入ボタンプッシュイベント //
        addLevel_TowerBlue
            .Subscribe(val =>
            {
                switch (val)
                {
                    case ShopData.TowerBlue_ParamList.Param_Trap:
                        mater.Value -= shopData.blueData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Trap.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Trap.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Trap.Value >= spLv.towerLv[(int)ShopData.TowerColor.Blue].MAX_LV)
                        {
                            btnTwB.SoldOutText.OnNext(ShopData.TowerBlue_ParamList.Param_Trap);
                        }
                        else
                        {
                            btnTwB.NextLv.OnNext(ShopData.TowerBlue_ParamList.Param_Trap);
                            btnTwB.ChangeLvText.OnNext(ShopData.TowerBlue_ParamList.Param_Trap);
                            btnTwB.ChangeValueText.OnNext(ShopData.TowerBlue_ParamList.Param_Trap);
                        }
                        break;
                    case ShopData.TowerBlue_ParamList.Param_Turret:
                        mater.Value -= shopData.blueData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Turret.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Turret.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Turret.Value >= spLv.towerLv[(int)ShopData.TowerColor.Blue].MAX_LV)
                        {
                            btnTwB.SoldOutText.OnNext(ShopData.TowerBlue_ParamList.Param_Turret);
                        }
                        else
                        {
                            btnTwB.NextLv.OnNext(ShopData.TowerBlue_ParamList.Param_Turret);
                            btnTwB.ChangeLvText.OnNext(ShopData.TowerBlue_ParamList.Param_Turret);
                            btnTwB.ChangeValueText.OnNext(ShopData.TowerBlue_ParamList.Param_Turret);
                        }
                        break;
                    case ShopData.TowerBlue_ParamList.Param_Tower:
                        mater.Value -= shopData.blueData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Tower.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Tower.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Tower.Value >= spLv.towerLv[(int)ShopData.TowerColor.Blue].MAX_LV)
                        {
                            btnTwB.SoldOutText.OnNext(ShopData.TowerBlue_ParamList.Param_Tower);
                        }
                        else
                        {
                            btnTwB.NextLv.OnNext(ShopData.TowerBlue_ParamList.Param_Tower);
                            btnTwB.ChangeLvText.OnNext(ShopData.TowerBlue_ParamList.Param_Tower);
                            btnTwB.ChangeValueText.OnNext(ShopData.TowerBlue_ParamList.Param_Tower);
                        }
                        break;
                    case ShopData.TowerBlue_ParamList.Repair:
                        mater.Value -= (shopData.blueData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Repair.Value + 1].purchaseMater * 5);
                        spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Repair.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Blue].level_Repair.Value >= spLv.MAX_LEVEL)
                        {
                            btnTwB.SoldOutText.OnNext(ShopData.TowerBlue_ParamList.Repair);
                        }
                        else
                        {
                            btnTwB.NextLv.OnNext(ShopData.TowerBlue_ParamList.Repair);
                            btnTwB.ChangeLvText.OnNext(ShopData.TowerBlue_ParamList.Repair);
                            btnTwB.ChangeValueText.OnNext(ShopData.TowerBlue_ParamList.Repair);
                        }
                        break;
                    default:
                        break;
                }
            }).AddTo(this.gameObject);

        // 黄タワー購入ボタンプッシュイベント //
        addLevel_TowerYellow
            .Subscribe(val =>
            {
                switch (val)
                {
                    case ShopData.TowerYellow_ParamList.Param_Trap:
                        mater.Value -= shopData.yellowData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Trap.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Trap.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Trap.Value >= spLv.towerLv[(int)ShopData.TowerColor.Yellow].MAX_LV)
                        {
                            btnTwY.SoldOutText.OnNext(ShopData.TowerYellow_ParamList.Param_Trap);
                        }
                        else
                        {
                            btnTwY.NextLv.OnNext(ShopData.TowerYellow_ParamList.Param_Trap);
                            btnTwY.ChangeLvText.OnNext(ShopData.TowerYellow_ParamList.Param_Trap);
                            btnTwY.ChangeValueText.OnNext(ShopData.TowerYellow_ParamList.Param_Trap);
                        }
                        break;
                    case ShopData.TowerYellow_ParamList.Param_Turret:
                        mater.Value -= shopData.yellowData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Turret.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Turret.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Turret.Value >= spLv.towerLv[(int)ShopData.TowerColor.Yellow].MAX_LV)
                        {
                            btnTwY.SoldOutText.OnNext(ShopData.TowerYellow_ParamList.Param_Turret);
                        }
                        else
                        {
                            btnTwY.NextLv.OnNext(ShopData.TowerYellow_ParamList.Param_Turret);
                            btnTwY.ChangeLvText.OnNext(ShopData.TowerYellow_ParamList.Param_Turret);
                            btnTwY.ChangeValueText.OnNext(ShopData.TowerYellow_ParamList.Param_Turret);
                        }
                        break;
                    case ShopData.TowerYellow_ParamList.Param_Tower:
                        mater.Value -= shopData.yellowData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Tower.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Tower.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Tower.Value >= spLv.towerLv[(int)ShopData.TowerColor.Yellow].MAX_LV)
                        {
                            btnTwY.SoldOutText.OnNext(ShopData.TowerYellow_ParamList.Param_Tower);
                        }
                        else
                        {
                            btnTwY.NextLv.OnNext(ShopData.TowerYellow_ParamList.Param_Tower);
                            btnTwY.ChangeLvText.OnNext(ShopData.TowerYellow_ParamList.Param_Tower);
                            btnTwY.ChangeValueText.OnNext(ShopData.TowerYellow_ParamList.Param_Tower);
                        }
                        break;
                    case ShopData.TowerYellow_ParamList.Repair:
                        mater.Value -= (shopData.yellowData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Repair.Value + 1].purchaseMater * 5);
                        spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Repair.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Yellow].level_Repair.Value >= spLv.MAX_LEVEL)
                        {
                            btnTwY.SoldOutText.OnNext(ShopData.TowerYellow_ParamList.Repair);
                        }
                        else
                        {
                            btnTwY.NextLv.OnNext(ShopData.TowerYellow_ParamList.Repair);
                            btnTwY.ChangeLvText.OnNext(ShopData.TowerYellow_ParamList.Repair);
                            btnTwY.ChangeValueText.OnNext(ShopData.TowerYellow_ParamList.Repair);
                        }
                        break;
                    default:
                        break;
                }
            }).AddTo(this.gameObject);

        // 緑タワー購入ボタンプッシュイベント //
        addLevel_TowerGreen
            .Subscribe(val =>
            {
                switch (val)
                {
                    case ShopData.TowerGreen_ParamList.Param_Trap:
                        mater.Value -= shopData.greenData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Green].level_Trap.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Green].level_Trap.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Green].level_Trap.Value >= spLv.towerLv[(int)ShopData.TowerColor.Green].MAX_LV)
                        {
                            btnTwG.SoldOutText.OnNext(ShopData.TowerGreen_ParamList.Param_Trap);
                        }
                        else
                        {
                            btnTwG.NextLv.OnNext(ShopData.TowerGreen_ParamList.Param_Trap);
                            btnTwG.ChangeLvText.OnNext(ShopData.TowerGreen_ParamList.Param_Trap);
                            btnTwG.ChangeValueText.OnNext(ShopData.TowerGreen_ParamList.Param_Trap);
                        }
                        break;
                    case ShopData.TowerGreen_ParamList.Param_Turret:
                        mater.Value -= shopData.greenData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Green].level_Turret.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Green].level_Turret.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Green].level_Turret.Value >= spLv.towerLv[(int)ShopData.TowerColor.Green].MAX_LV)
                        {
                            btnTwG.SoldOutText.OnNext(ShopData.TowerGreen_ParamList.Param_Turret);
                        }
                        else
                        {
                            btnTwG.NextLv.OnNext(ShopData.TowerGreen_ParamList.Param_Turret);
                            btnTwG.ChangeLvText.OnNext(ShopData.TowerGreen_ParamList.Param_Turret);
                            btnTwG.ChangeValueText.OnNext(ShopData.TowerGreen_ParamList.Param_Turret);
                        }
                        break;
                    case ShopData.TowerGreen_ParamList.Param_Tower:
                        mater.Value -= shopData.greenData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Green].level_Tower.Value + 1].purchaseMater;
                        spLv.towerLv[(int)ShopData.TowerColor.Green].level_Tower.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Green].level_Tower.Value >= spLv.towerLv[(int)ShopData.TowerColor.Yellow].MAX_LV)
                        {
                            btnTwG.SoldOutText.OnNext(ShopData.TowerGreen_ParamList.Param_Tower);
                        }
                        else
                        {
                            btnTwG.NextLv.OnNext(ShopData.TowerGreen_ParamList.Param_Tower);
                            btnTwG.ChangeLvText.OnNext(ShopData.TowerGreen_ParamList.Param_Tower);
                            btnTwG.ChangeValueText.OnNext(ShopData.TowerGreen_ParamList.Param_Tower);
                        }
                        break;
                    case ShopData.TowerGreen_ParamList.Repair:
                        mater.Value -= (shopData.greenData_Tower[spLv.towerLv[(int)ShopData.TowerColor.Green].level_Repair.Value + 1].purchaseMater * 5);
                        spLv.towerLv[(int)ShopData.TowerColor.Green].level_Repair.Value++;
                        if (spLv.towerLv[(int)ShopData.TowerColor.Green].level_Repair.Value >= spLv.MAX_LEVEL)
                        {
                            btnTwG.SoldOutText.OnNext(ShopData.TowerGreen_ParamList.Repair);
                        }
                        else
                        {
                            btnTwG.NextLv.OnNext(ShopData.TowerGreen_ParamList.Repair);
                            btnTwG.ChangeLvText.OnNext(ShopData.TowerGreen_ParamList.Repair);
                            btnTwG.ChangeValueText.OnNext(ShopData.TowerGreen_ParamList.Repair);
                        }
                        break;
                    default:
                        break;
                }
            }).AddTo(this.gameObject);


        addLevel_Skill
            .Subscribe(val =>
            {
                switch (val)
                {
                    case ShopData.Skill_ParamList.Normal:
                        btnSkill.normal.Value = true;
                        break;
                    case ShopData.Skill_ParamList.Razer:
                        btnSkill.razer.Value = true;
                        break;
                    case ShopData.Skill_ParamList.Missile:
                        btnSkill.missile.Value = true;
                        break;
                    case ShopData.Skill_ParamList.Bomb:
                        btnSkill.bomb.Value = true;
                        break;
                    default:
                        break;
                }
                mater.Value -= shopData.shopData_Skill[(int)val].purchaseMater;
                spLv.skillLv.level_Skill.Value = (int)val;
            }).AddTo(this.gameObject);

        addLevel_Ult
            .Subscribe(val =>
            {
                switch (val)
                {
                    case ShopData.Ult_ParamList.Normal:
                        btnUlt.normal.Value = true;
                        break;
                    case ShopData.Ult_ParamList.Trap:
                        btnUlt.trap.Value = true;
                        break;
                    case ShopData.Ult_ParamList.Bomb:
                        btnUlt.bomb.Value = true;
                        break;
                    case ShopData.Ult_ParamList.Repair:
                        btnUlt.repair.Value = true;
                        break;
                    default:
                        break;
                }
                mater.Value -= shopData.shopData_Ult[(int)val].purchaseMater;
                spLv.ultLv.level_Ult.Value = (int)val;
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
