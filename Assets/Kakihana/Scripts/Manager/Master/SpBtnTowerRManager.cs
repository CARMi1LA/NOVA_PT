using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnTowerRManager : MonoBehaviour
{
    // ショップの赤タワー購入画面の管理クラス
    // 基本はShopManagerやShopByControllerの命令により各処理を実行する
    private LevelData_Tower redTower_Lv;
    public ShopBtnManager[] spBtn;

    private ShopData.TowerRed_ParamList param;

    // 次のレベルを保存する変数
    private int nextLvTrap;
    private int nextLvTurret;
    private int nextLvTower;
    private int nextLvRepair;
    private int nextRepairVal;

    // 初期化イベント
    public Subject<LevelData_Tower> InitSubject = new Subject<LevelData_Tower>();
    // レベル更新イベント
    public Subject<ShopData.TowerRed_ParamList> ChangeLvText = new Subject<ShopData.TowerRed_ParamList>();
    // 必要金額更新イベント
    public Subject<ShopData.TowerRed_ParamList> ChangeValueText = new Subject<ShopData.TowerRed_ParamList>();
    // 購入可能時のイベント
    public Subject<ShopData.TowerRed_ParamList> BuyOkText = new Subject<ShopData.TowerRed_ParamList>();
    // 購入不可能時のイベント
    public Subject<ShopData.TowerRed_ParamList> BuyNgText = new Subject<ShopData.TowerRed_ParamList>();
    // レベル最大時のイベント
    public Subject<ShopData.TowerRed_ParamList> SoldOutText = new Subject<ShopData.TowerRed_ParamList>();
    // 次レベル設定イベント
    public Subject<ShopData.TowerRed_ParamList> NextLv = new Subject<ShopData.TowerRed_ParamList>();
    // Start is called before the first frame update
    void Start()
    {
        // UIで使用するため次レベルを保存しておく
        NextLv.Subscribe(_ =>
        {
            switch (_)
            {
                case ShopData.TowerRed_ParamList.Param_Trap:
                    nextLvTrap = ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value + 1;
                    break;
                case ShopData.TowerRed_ParamList.Param_Turret:
                    nextLvTurret = ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value + 1;
                    break;
                case ShopData.TowerRed_ParamList.Param_Tower:
                    nextLvTower = ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value + 1;
                    break;
                case ShopData.TowerRed_ParamList.Repair:
                    nextLvRepair = ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Repair.Value + 1;
                    nextRepairVal = (ShopManager.Instance.shopData.redData_Tower[nextLvRepair].purchaseMater * 5);
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);

        // 初期化処理
        InitSubject.Subscribe(_ =>
        {
            Debug.Log("ken1111");
            // プレイヤーのレベルデータを設定
            redTower_Lv = _;

            // 次のレベルを設定
            NextLv.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
            NextLv.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
            NextLv.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
            NextLv.OnNext(ShopData.TowerRed_ParamList.Repair);

            // 強化内容テキストを設定
            spBtn[0].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value,
                nextLvTrap);

            spBtn[1].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value,
                nextLvTurret);

            spBtn[2].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value,
                nextLvTower);

            spBtn[3].levelText.text =
                string.Format("{0}回目",nextLvRepair);

            // 必要金額テキストを設定
            spBtn[0].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                redData_Tower[nextLvTrap].purchaseMater);

            spBtn[1].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                redData_Tower[nextLvTurret].purchaseMater);


            spBtn[2].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                redData_Tower[nextLvTower].purchaseMater);

            spBtn[3].materValueText.text =
                string.Format("{0}",nextRepairVal);
        }).AddTo(this.gameObject);

        // 購入可能時のイベント
        BuyOkText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.TowerRed_ParamList.Param_Trap:
                    // 色を通常に
                    spBtn[0].materValueText.color = Color.black;
                    // ボタンを押せるようにする
                    spBtn[0].myBtn.interactable = true;
                    break;
                case ShopData.TowerRed_ParamList.Param_Turret:
                    spBtn[1].materValueText.color = Color.black;
                    spBtn[1].myBtn.interactable = true;
                    break;
                case ShopData.TowerRed_ParamList.Param_Tower:
                    spBtn[2].materValueText.color = Color.black;
                    spBtn[2].myBtn.interactable = true;
                    break;
                case ShopData.TowerRed_ParamList.Repair:
                    spBtn[3].materValueText.color = Color.black;
                    spBtn[3].myBtn.interactable = true;
                    break;
            }
        }).AddTo(this.gameObject);
        // 購入不可能時のイベント
        BuyNgText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.TowerRed_ParamList.Param_Trap:
                    // 文字を赤色に
                    spBtn[0].materValueText.color = Color.red;
                    // ボタンを押せなくなるようにする
                    spBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.TowerRed_ParamList.Param_Turret:
                    spBtn[1].materValueText.color = Color.red;
                    spBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.TowerRed_ParamList.Param_Tower:
                    spBtn[2].materValueText.color = Color.red;
                    spBtn[2].myBtn.interactable = false;
                    break;
                case ShopData.TowerRed_ParamList.Repair:
                    spBtn[3].materValueText.color = Color.red;
                    spBtn[3].myBtn.interactable = false;
                    break;
            }
        }).AddTo(this.gameObject);

        // 購入内容更新イベント
        ChangeLvText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.TowerRed_ParamList.Param_Trap:
                    // 購入内容テキストを更新する
                    // 表示内容：（現在のLv→次のLv）
                    spBtn[0].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value,
                        nextLvTrap);
                    break;
                case ShopData.TowerRed_ParamList.Param_Turret:
                    spBtn[1].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value,
                        nextLvTurret);
                    break;
                case ShopData.TowerRed_ParamList.Param_Tower:
                    spBtn[2].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        ShopManager.Instance.spLv.towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value,
                        nextLvTower);
                    break;
                case ShopData.TowerRed_ParamList.Repair:
                    spBtn[3].levelText.text =
                    string.Format("{0}回目",nextLvRepair);
                    break;
            }
        }).AddTo(this.gameObject);

        // 必要金額更新イベント
        ChangeValueText.Subscribe(list =>
        {
            switch (list)
            {
                // 必要金額のUIを変更する
                case ShopData.TowerRed_ParamList.Param_Trap:
                    spBtn[0].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        redData_Tower[nextLvTrap].purchaseMater);
                    break;
                case ShopData.TowerRed_ParamList.Param_Turret:
                    spBtn[1].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        redData_Tower[nextLvTurret].purchaseMater);
                    break;
                case ShopData.TowerRed_ParamList.Param_Tower:
                    spBtn[2].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        redData_Tower[nextLvTower].purchaseMater);
                    break;
                case ShopData.TowerRed_ParamList.Repair:
                    spBtn[3].materValueText.text =
                    string.Format("{0}",nextRepairVal);
                    break;
            }
        }).AddTo(this.gameObject);

        // レベル最大時のイベント
        SoldOutText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.TowerRed_ParamList.Param_Trap:
                    // 必要金額UIに売り切れを表示させる
                    spBtn[0].materValueText.text = string.Format("SOLD OUT");
                    // 現在のレベルUIにレベル最大を表示させる
                    spBtn[0].levelText.text = string.Format("LvMAX!");
                    // 文字を赤色にする
                    spBtn[0].materValueText.color = Color.red;
                    // ボタンが押せなくなるようにする
                    spBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.TowerRed_ParamList.Param_Turret:
                    spBtn[1].materValueText.text = string.Format("SOLD OUT");
                    spBtn[1].levelText.text = string.Format("LvMAX!");
                    spBtn[1].materValueText.color = Color.red;
                    spBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.TowerRed_ParamList.Param_Tower:
                    spBtn[2].materValueText.text = string.Format("SOLD OUT");
                    spBtn[2].levelText.text = string.Format("LvMAX!");
                    spBtn[2].materValueText.color = Color.red;
                    spBtn[2].myBtn.interactable = false;
                    break;
                case ShopData.TowerRed_ParamList.Repair:
                    spBtn[3].materValueText.text = string.Format("SOLD OUT");
                    spBtn[3].levelText.text = string.Format("Max Repaired...");
                    spBtn[3].materValueText.color = Color.red;
                    spBtn[3].myBtn.interactable = false;
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);
    }
}
