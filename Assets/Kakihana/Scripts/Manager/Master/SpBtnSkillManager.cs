using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnSkillManager : MonoBehaviour
{
    private LevelData_Skill skillData_Lv;
    public ShopBtnManager[] spBtn;

    public BoolReactiveProperty normal = new BoolReactiveProperty(false);
    public BoolReactiveProperty razer = new BoolReactiveProperty(false);
    public BoolReactiveProperty missile = new BoolReactiveProperty(false);
    public BoolReactiveProperty bomb = new BoolReactiveProperty(false);

    // 初期化イベント
    public Subject<LevelData_Skill> InitSubject = new Subject<LevelData_Skill>();
    // 購入画面のイベント
    public Subject<ShopData.Skill_ParamList> buyInfoText = new Subject<ShopData.Skill_ParamList>();
    // 購入可能時のイベント
    public Subject<ShopData.Skill_ParamList> BuyOkText = new Subject<ShopData.Skill_ParamList>();
    // 購入不可能時のイベント
    public Subject<ShopData.Skill_ParamList> BuyNgText = new Subject<ShopData.Skill_ParamList>();
    // レベル最大時のイベント
    public Subject<ShopData.Skill_ParamList> SoldOutText = new Subject<ShopData.Skill_ParamList>();
    // Start is called before the first frame update
    void Start()
    {
        normal.Where(_ => normal.Value == true)
            .Subscribe(_ => 
            {
                buyInfoText.OnNext(ShopData.Skill_ParamList.Normal);
            }).AddTo(this.gameObject);

        normal.Where(_ => normal.Value == false)
            .Subscribe(_ =>
            {
                spBtn[0].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Skill[(int)ShopData.Skill_ParamList.Normal].purchaseMater);
                spBtn[0].levelText.text = string.Format("前方に範囲攻撃");
            }).AddTo(this.gameObject);

        razer.Where(_ => razer.Value == true)
            .Subscribe(_ => 
            {
                buyInfoText.OnNext(ShopData.Skill_ParamList.Razer);
            }).AddTo(this.gameObject);
        razer.Where(_ => razer.Value == false)
            .Subscribe(_ =>
            {
                spBtn[1].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Skill[(int)ShopData.Skill_ParamList.Razer].purchaseMater);
                spBtn[1].levelText.text = string.Format("直線の貫通攻撃");
            }).AddTo(this.gameObject);

        missile.Where(_ => missile.Value == true)
            .Subscribe(_ =>
            {
                buyInfoText.OnNext(ShopData.Skill_ParamList.Missile);
            }).AddTo(this.gameObject);

        missile.Where(_ => missile.Value == false)
            .Subscribe(_ =>
            {
                spBtn[2].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Skill[(int)ShopData.Skill_ParamList.Missile].purchaseMater);
                spBtn[2].levelText.text = string.Format("敵単体に必中攻撃");
            }).AddTo(this.gameObject);

        bomb.Where(_ => bomb.Value == true)
            .Subscribe(_ =>
            {
                buyInfoText.OnNext(ShopData.Skill_ParamList.Bomb);
            }).AddTo(this.gameObject);

        bomb.Where(_ => bomb.Value == false)
            .Subscribe(_ =>
            {
                spBtn[3].materValueText.text = string.Format("{0}", ShopManager.Instance.shopData.shopData_Skill[(int)ShopData.Skill_ParamList.Bomb].purchaseMater);
                spBtn[3].levelText.text = string.Format("着弾で広範囲爆撃");
            }).AddTo(this.gameObject);
        // 購入可能時のイベント
        BuyOkText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Skill_ParamList.Normal:
                    // 色を通常に
                    spBtn[0].materValueText.color = Color.black;
                    // ボタンを押せるようにする
                    spBtn[0].myBtn.interactable = true;
                    break;
                case ShopData.Skill_ParamList.Razer:
                    spBtn[1].materValueText.color = Color.black;
                    spBtn[1].myBtn.interactable = true;
                    break;
                case ShopData.Skill_ParamList.Missile:
                    spBtn[2].materValueText.color = Color.black;
                    spBtn[2].myBtn.interactable = true;
                    break;
                case ShopData.Skill_ParamList.Bomb:
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
                case ShopData.Skill_ParamList.Normal:
                    // 文字を赤色に
                    spBtn[0].materValueText.color = Color.red;
                    // ボタンを押せなくなるようにする
                    spBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.Skill_ParamList.Razer:
                    spBtn[1].materValueText.color = Color.red;
                    spBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.Skill_ParamList.Missile:
                    spBtn[2].materValueText.color = Color.red;
                    spBtn[2].myBtn.interactable = false;
                    break;
                case ShopData.Skill_ParamList.Bomb:
                    spBtn[3].materValueText.color = Color.red;
                    spBtn[3].myBtn.interactable = false;
                    break;
            }
        }).AddTo(this.gameObject);

        buyInfoText.Subscribe(list => 
        {
            switch (list)
            {
                case ShopData.Skill_ParamList.Normal:
                    SoldOutText.OnNext(ShopData.Skill_ParamList.Normal);
                    razer.Value = false;
                    missile.Value = false;
                    bomb.Value = false;
                    break;
                case ShopData.Skill_ParamList.Razer:
                    SoldOutText.OnNext(list);
                    normal.Value = false;
                    missile.Value = false;
                    bomb.Value = false;
                    break;
                case ShopData.Skill_ParamList.Missile:
                    SoldOutText.OnNext(list);
                    normal.Value = false;
                    razer.Value = false;
                    bomb.Value = false;
                    break;
                case ShopData.Skill_ParamList.Bomb:
                    SoldOutText.OnNext(list);
                    normal.Value = false;
                    razer.Value = false;
                    missile.Value = false;
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);

        // レベル最大時のイベント
        SoldOutText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Skill_ParamList.Normal:
                    // 必要金額UIに売り切れを表示させる
                    spBtn[0].materValueText.text = string.Format("SOLD OUT");
                    // 現在のレベルUIにレベル最大を表示させる
                    spBtn[0].levelText.text = string.Format("Equipped...");
                    // 文字を赤色にする
                    spBtn[0].materValueText.color = Color.red;
                    // ボタンが押せなくなるようにする
                    spBtn[0].myBtn.interactable = false;
                    break;
                case ShopData.Skill_ParamList.Razer:
                    spBtn[1].materValueText.text = string.Format("SOLD OUT");
                    spBtn[1].levelText.text = string.Format("Equipped...!");
                    spBtn[1].materValueText.color = Color.red;
                    spBtn[1].myBtn.interactable = false;
                    break;
                case ShopData.Skill_ParamList.Missile:
                    spBtn[2].materValueText.text = string.Format("SOLD OUT");
                    spBtn[2].levelText.text = string.Format("Equipped...");
                    spBtn[2].materValueText.color = Color.red;
                    spBtn[2].myBtn.interactable = false;
                    break;
                case ShopData.Skill_ParamList.Bomb:
                    spBtn[3].materValueText.text = string.Format("SOLD OUT");
                    spBtn[3].levelText.text = string.Format("Equipped...");
                    spBtn[3].materValueText.color = Color.red;
                    spBtn[3].myBtn.interactable = false;
                    break;
                default:
                    break;
            }
        }).AddTo(this.gameObject);
    }
}
