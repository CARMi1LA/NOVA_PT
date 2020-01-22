using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnPlayerManager : MonoBehaviour
{
    // ショップのプレイヤー購入画面の管理クラス
    // 基本はShopManagerやShopByControllerの命令により各処理を実行する

    // プレイヤーレベル
    public LevelData_Player levelData_Player;
    // プレイヤー購入画面のボタン
    public ShopBtnManager[] spPlayerBtn;
    // 次のレベルを保存する変数
    private int nextLvHp;
    private int nextLvSpd;
    private int nextLvInt;

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
    private void Awake()
    {
        // UIで使用するため次レベルを保存しておく
        NextLv.Subscribe(_ =>
        {
            Debug.Log("NextLvPlayer");
            switch (_)
            {
                case ShopData.Player_ParamList.Param_HP:
                    nextLvHp = ShopManager.Instance.spLv.playerLv.lv_HP.Value + 1;
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    nextLvSpd = ShopManager.Instance.spLv.playerLv.lv_Spd.Value + 1;
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    nextLvInt = ShopManager.Instance.spLv.playerLv.lv_Int.Value + 1;
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);

        // 初期化処理
        InitSubject.Subscribe(_ =>
        {
            Debug.Log("initbtnPlayer");
            // プレイヤーのレベルデータを設定
            levelData_Player = _;

            // 次のレベルを設定
            NextLv.OnNext(ShopData.Player_ParamList.Param_HP);
            NextLv.OnNext(ShopData.Player_ParamList.Param_Speed);
            NextLv.OnNext(ShopData.Player_ParamList.Param_Interval);

            // 強化内容テキストを設定
            spPlayerBtn[0].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                ShopManager.Instance.spLv.playerLv.lv_HP.Value,
                nextLvHp);

            spPlayerBtn[1].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                ShopManager.Instance.spLv.playerLv.lv_Spd.Value,
                nextLvSpd);

            spPlayerBtn[2].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                ShopManager.Instance.spLv.playerLv.lv_Int.Value,
                nextLvInt);

            // 必要金額テキストを設定
            spPlayerBtn[0].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                shopData_Player[nextLvHp].purchaseMater);

            spPlayerBtn[1].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                shopData_Player[nextLvSpd].purchaseMater);

            spPlayerBtn[2].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                shopData_Player[nextLvInt].purchaseMater);
        }).AddTo(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

        InitSubject.OnNext(ShopManager.Instance.spLv.playerLv);

        // 購入可能時のイベント
        BuyOkText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    // 色を通常に
                    spPlayerBtn[0].materValueText.color = Color.black;
                    // ボタンを押せるようにする
                    spPlayerBtn[0].myBtn.interactable = true;
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].materValueText.color = Color.black;
                    spPlayerBtn[1].myBtn.interactable = true;
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].materValueText.color = Color.black;
                    spPlayerBtn[2].myBtn.interactable = true;
                    break;
            }
        }).AddTo(this.gameObject);

        // 購入不可能時のイベント
        BuyNgText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    // 文字を赤色に
                    spPlayerBtn[0].materValueText.color = Color.red;
                    // ボタンを押せなくなるようにする
                    spPlayerBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].materValueText.color = Color.red;
                    spPlayerBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].materValueText.color = Color.red;
                    spPlayerBtn[2].myBtn.interactable = false;
                    break;
            }
        }).AddTo(this.gameObject);

        // 購入内容更新イベント
        ChangeLvText.Subscribe(list => 
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    // 購入内容テキストを更新する
                    // 表示内容：（現在のLv→次のLv）
                    spPlayerBtn[0].levelText.text = 
                        string.Format("Lv{0}→Lv{1}",
                        ShopManager.Instance.spLv.playerLv.lv_HP.Value,
                        nextLvHp);
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        ShopManager.Instance.spLv.playerLv.lv_Spd.Value,
                        nextLvSpd);
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        ShopManager.Instance.spLv.playerLv.lv_Int.Value,
                        nextLvInt);
                    break;
            }
        }).AddTo(this.gameObject);

        // 必要金額更新イベント
        ChangeValueText.Subscribe(list =>
        {
            switch (list)
            {
                // 必要金額のUIを変更する
                case ShopData.Player_ParamList.Param_HP:
                    spPlayerBtn[0].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        shopData_Player[nextLvHp].purchaseMater);
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        shopData_Player[nextLvSpd].purchaseMater);
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        shopData_Player[nextLvInt].purchaseMater);
                    break;
            }
        }).AddTo(this.gameObject);

        // レベル最大時のイベント
        SoldOutText.Subscribe(list => 
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    // 必要金額UIに売り切れを表示させる
                    spPlayerBtn[0].materValueText.text = string.Format("SOLD OUT");
                    // 現在のレベルUIにレベル最大を表示させる
                    spPlayerBtn[0].levelText.text = string.Format("LvMAX!");
                    // 文字を赤色にする
                    spPlayerBtn[0].materValueText.color = Color.red;
                    // ボタンが押せなくなるようにする
                    spPlayerBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].materValueText.text = string.Format("SOLD OUT");
                    spPlayerBtn[1].levelText.text = string.Format("LvMAX!");
                    spPlayerBtn[1].materValueText.color = Color.red;
                    spPlayerBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].materValueText.text = string.Format("SOLD OUT");
                    spPlayerBtn[2].levelText.text = string.Format("LvMAX!");
                    spPlayerBtn[2].materValueText.color = Color.red;
                    spPlayerBtn[2].myBtn.interactable = false;
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);

    }
}
