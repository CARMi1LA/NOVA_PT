using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnPlayerManager : MonoBehaviour
{
    public LevelData_Player levelData_Player;

    public ShopBtnManager[] spPlayerBtn;

    private int nextLvHp;
    private int nextLvSpd;
    private int nextLvInt;


    public Subject<LevelData_Player> InitSubject = new Subject<LevelData_Player>();
    public Subject<ShopData.Player_ParamList> ChangeLvText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> ChangeValueText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> BuyOkText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> BuyNgText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> SoldOutText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> NextLv = new Subject<ShopData.Player_ParamList>();
    // Start is called before the first frame update
    void Start()
    {
        NextLv.Subscribe(_ => 
        {
            switch (_)
            {
                case ShopData.Player_ParamList.Param_HP:
                    if (ShopManager.Instance.spLv.playerLv.lv_HP.Value >= ShopManager.Instance.spLv.playerLv.MAX_LV)
                    {
                        SoldOutText.OnNext(_);
                    }
                    else
                    {
                        nextLvHp = ShopManager.Instance.spLv.playerLv.lv_HP.Value + 1;
                    }
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

        InitSubject.Subscribe(_ => 
        {
            Debug.Log("SPbtnInit");
            levelData_Player = _;

            NextLv.OnNext(ShopData.Player_ParamList.Param_HP);
            NextLv.OnNext(ShopData.Player_ParamList.Param_Speed);
            NextLv.OnNext(ShopData.Player_ParamList.Param_Interval);

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

        BuyOkText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    spPlayerBtn[0].materValueText.color = Color.black;
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

        BuyNgText.Subscribe(list =>
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    spPlayerBtn[0].materValueText.color = Color.red;
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

        ChangeLvText.Subscribe(list => 
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
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

        ChangeValueText.Subscribe(list =>
        {
            switch (list)
            {
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

        SoldOutText.Subscribe(list => 
        {
            switch (list)
            {
                case ShopData.Player_ParamList.Param_HP:
                    spPlayerBtn[0].materValueText.text = string.Format("SOLD OUT");
                    spPlayerBtn[0].levelText.text = string.Format("LvMAX!");
                    spPlayerBtn[0].materValueText.color = Color.red;
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
