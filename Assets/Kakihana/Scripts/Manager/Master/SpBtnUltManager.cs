using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnUltManager : MonoBehaviour
{
    private LevelData_Ult ultData_Lv;
    public ShopBtnManager[] spBtn;

    public BoolReactiveProperty normal = new BoolReactiveProperty(false);
    public BoolReactiveProperty trap = new BoolReactiveProperty(false);
    public BoolReactiveProperty bomb = new BoolReactiveProperty(false);
    public BoolReactiveProperty repair = new BoolReactiveProperty(false);

    // 初期化イベント
    public Subject<Unit> InitSubject = new Subject<Unit>();
    // 購入画面のイベント
    public Subject<ShopData.Ult_ParamList> buyInfoText = new Subject<ShopData.Ult_ParamList>();
    // 購入可能時のイベント
    public Subject<ShopData.Ult_ParamList> BuyOkText = new Subject<ShopData.Ult_ParamList>();
    // 購入不可能時のイベント
    public Subject<ShopData.Ult_ParamList> BuyNgText = new Subject<ShopData.Ult_ParamList>();
    // レベル最大時のイベント
    public Subject<ShopData.Ult_ParamList> SoldOutText = new Subject<ShopData.Ult_ParamList>();

    private void Awake()
    {
        // レベル最大時のイベント
        SoldOutText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Ult_ParamList.Normal:
                    // 必要金額UIに売り切れを表示させる
                    spBtn[0].materValueText.text = string.Format("SOLD OUT");
                    // 現在のレベルUIにレベル最大を表示させる
                    spBtn[0].levelText.text = string.Format("Equipped...");
                    // 文字を赤色にする
                    spBtn[0].materValueText.color = Color.red;
                    // ボタンが押せなくなるようにする
                    spBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.Ult_ParamList.Trap:
                    spBtn[1].materValueText.text = string.Format("SOLD OUT");
                    spBtn[1].levelText.text = string.Format("Equipped...!");
                    spBtn[1].materValueText.color = Color.red;
                    spBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.Ult_ParamList.Bomb:
                    spBtn[2].materValueText.text = string.Format("SOLD OUT");
                    spBtn[2].levelText.text = string.Format("Equipped...");
                    spBtn[2].materValueText.color = Color.red;
                    spBtn[2].myBtn.interactable = false;
                    break;
                case ShopData.Ult_ParamList.Repair:
                    spBtn[3].materValueText.text = string.Format("SOLD OUT");
                    spBtn[3].levelText.text = string.Format("Equipped...");
                    spBtn[3].materValueText.color = Color.red;
                    spBtn[3].myBtn.interactable = false;
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);

        InitSubject.Subscribe(_ => 
        {
            spBtn[0].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Ult[(int)ShopData.Ult_ParamList.Normal].purchaseMater);
            SoldOutText.OnNext(ShopData.Ult_ParamList.Normal);
            trap.Value = false;
            bomb.Value = false;
            repair.Value = false;
        }).AddTo(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
        {
            InitSubject.OnNext(Unit.Default);

            normal.Where(_ => normal.Value == true)
                 .Subscribe(_ =>
                 {
                     buyInfoText.OnNext(ShopData.Ult_ParamList.Normal);
                 }).AddTo(this.gameObject);
            normal.Where(_ => normal.Value == false)
                .Subscribe(_ =>
                {
                    spBtn[0].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Ult[(int)ShopData.Ult_ParamList.Normal].purchaseMater);
                    spBtn[0].levelText.text = string.Format("一定時間無敵になる");
                }).AddTo(this.gameObject);

            trap.Where(_ => trap.Value == true)
                .Subscribe(_ =>
                {
                    buyInfoText.OnNext(ShopData.Ult_ParamList.Trap);
                }).AddTo(this.gameObject);
            trap.Where(_ => trap.Value == false)
                .Subscribe(_ =>
                {
                    spBtn[1].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Ult[(int)ShopData.Ult_ParamList.Trap].purchaseMater);
                    spBtn[1].levelText.text = string.Format("スロートラップを設置する");
                }).AddTo(this.gameObject);

            bomb.Where(_ => bomb.Value == true)
                .Subscribe(_ =>
                {
                    buyInfoText.OnNext(ShopData.Ult_ParamList.Bomb);
                }).AddTo(this.gameObject);

            bomb.Where(_ => bomb.Value == false)
                .Subscribe(_ =>
                {
                    spBtn[2].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Ult[(int)ShopData.Ult_ParamList.Bomb].purchaseMater);
                    spBtn[2].levelText.text = string.Format("広範囲爆撃を行う");
                }).AddTo(this.gameObject);

            repair.Where(_ => repair.Value == true)
                .Subscribe(_ =>
                {
                    buyInfoText.OnNext(ShopData.Ult_ParamList.Repair);
                }).AddTo(this.gameObject);

            repair.Where(_ => repair.Value == false)
                .Subscribe(_ =>
                {
                    spBtn[3].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Ult[(int)ShopData.Ult_ParamList.Repair].purchaseMater);
                    spBtn[3].levelText.text = string.Format("全タワーの耐久値を回復");
                }).AddTo(this.gameObject);
            // 購入可能時のイベント
            BuyOkText.Subscribe(list =>
            {
                switch (list)
                {
                    case ShopData.Ult_ParamList.Normal:
                    // 色を通常に
                    spBtn[0].materValueText.color = Color.black;
                    // ボタンを押せるようにする
                    spBtn[0].myBtn.interactable = true;
                        break;
                    case ShopData.Ult_ParamList.Trap:
                        spBtn[1].materValueText.color = Color.black;
                        spBtn[1].myBtn.interactable = true;
                        break;
                    case ShopData.Ult_ParamList.Bomb:
                        spBtn[2].materValueText.color = Color.black;
                        spBtn[2].myBtn.interactable = true;
                        break;
                    case ShopData.Ult_ParamList.Repair:
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
                    case ShopData.Ult_ParamList.Normal:
                    // 文字を赤色に
                    spBtn[0].materValueText.color = Color.red;
                    // ボタンを押せなくなるようにする
                    spBtn[0].myBtn.interactable = false;
                        break;
                    case ShopData.Ult_ParamList.Trap:
                        spBtn[1].materValueText.color = Color.red;
                        spBtn[1].myBtn.interactable = false;
                        break;
                    case ShopData.Ult_ParamList.Bomb:
                        spBtn[2].materValueText.color = Color.red;
                        spBtn[2].myBtn.interactable = false;
                        break;
                    case ShopData.Ult_ParamList.Repair:
                        spBtn[3].materValueText.color = Color.red;
                        spBtn[3].myBtn.interactable = false;
                        break;
                }
            }).AddTo(this.gameObject);

            buyInfoText.Subscribe(list =>
            {
                switch (list)
                {
                    case ShopData.Ult_ParamList.Normal:
                        SoldOutText.OnNext(ShopData.Ult_ParamList.Normal);
                        trap.Value = false;
                        bomb.Value = false;
                        repair.Value = false;
                        break;
                    case ShopData.Ult_ParamList.Trap:
                        SoldOutText.OnNext(list);
                        normal.Value = false;
                        bomb.Value = false;
                        repair.Value = false;
                        break;
                    case ShopData.Ult_ParamList.Bomb:
                        SoldOutText.OnNext(list);
                        normal.Value = false;
                        trap.Value = false;
                        repair.Value = false;
                        break;
                    case ShopData.Ult_ParamList.Repair:
                        SoldOutText.OnNext(list);
                        normal.Value = false;
                        trap.Value = false;
                        bomb.Value = false;
                        break;
                    default:
                        break;
                }
            }).AddTo(this.gameObject);
        }
}
