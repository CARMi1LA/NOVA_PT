using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnPlayerManager : MonoBehaviour
{
    public LevelData_Player levelData_Player;

    public ShopBtnManager[] spPlayerBtn;

    public Subject<LevelData_Player> InitSubject = new Subject<LevelData_Player>();
    public Subject<ShopData.Player_ParamList> ChangeLvText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> ChangeValueText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> BuyOkText = new Subject<ShopData.Player_ParamList>();
    public Subject<ShopData.Player_ParamList> BuyNgText = new Subject<ShopData.Player_ParamList>();
    Subject<ShopData.Player_ParamList> SoldOutText = new Subject<ShopData.Player_ParamList>();
    // Start is called before the first frame update
    void Start()
    {
        InitSubject.Subscribe(_ => 
        {
            levelData_Player = _;

            spPlayerBtn[0].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                levelData_Player.level_HP.Value,
                levelData_Player.level_HP.Value + 1);

            spPlayerBtn[1].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                levelData_Player.level_Speed.Value,
                levelData_Player.level_Speed.Value + 1);

            spPlayerBtn[2].levelText.text =
                string.Format("Lv{0}→Lv{1}",
                levelData_Player.level_Interval.Value,
                levelData_Player.level_Interval.Value + 1);

            spPlayerBtn[0].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                shopData_Player[levelData_Player.level_HP.Value + 1].purchaseMater);

            spPlayerBtn[1].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                shopData_Player[levelData_Player.level_Speed.Value + 1].purchaseMater);

            spPlayerBtn[2].materValueText.text =
                string.Format("{0}",
                ShopManager.Instance.shopData.
                shopData_Player[levelData_Player.level_Interval.Value + 1].purchaseMater);
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
                        levelData_Player.level_HP.Value, 
                        levelData_Player.level_HP.Value + 1);
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        levelData_Player.level_Speed.Value,
                        levelData_Player.level_Speed.Value + 1);
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].levelText.text =
                        string.Format("Lv{0}→Lv{1}",
                        levelData_Player.level_Interval.Value,
                        levelData_Player.level_Interval.Value + 1);
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
                        shopData_Player[levelData_Player.level_HP.Value + 1].purchaseMater);
                    break;
                case ShopData.Player_ParamList.Param_Speed:
                    spPlayerBtn[1].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        shopData_Player[levelData_Player.level_Speed.Value + 1].purchaseMater);
                    break;
                case ShopData.Player_ParamList.Param_Interval:
                    spPlayerBtn[2].materValueText.text =
                        string.Format("{0}",
                        ShopManager.Instance.shopData.
                        shopData_Player[levelData_Player.level_Interval.Value + 1].purchaseMater);
                    break;
            }
        }).AddTo(this.gameObject);


    }
}
