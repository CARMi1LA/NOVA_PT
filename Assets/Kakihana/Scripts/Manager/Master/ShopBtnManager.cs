using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class ShopBtnManager : MonoBehaviour
{
    public enum BtnGroup
    {
        Player = 0,
        Tower,
        Skill,
        Ult
    }

    public enum TowerColor
    {
        Red = 0,
        Blue,
        Yellow,
        Green
    }

    public BtnGroup btnGroup;       // ボタンのカテゴリー
    public TowerColor towerColor;   // タワーカテゴリの場合、所属する色の設定
    public ShopData.Player_ParamList player_ParamList;
    public ShopData.TowerRed_ParamList tower_ParamList;
    public ShopData.Skill_ParamList skill_ParamList;
    public ShopData.Ult_ParamList ult_ParamList;

    public ShopData shopData;

    public Button myBtn;
    public Text productName;        // 商品名テキスト
    public Text levelText;          // 現在の商品ごとのレベル
    public Text materValueText;     // 購入価格

    // Start is called before the first frame update
    void Start()
    {
        shopData = ShopManager.Instance.shopData;
        switch (btnGroup)
        {
            case BtnGroup.Player:
                switch (player_ParamList)
                {
                    case ShopData.Player_ParamList.Param_HP:
                        
                        break;
                    case ShopData.Player_ParamList.Param_Speed:
                        break;
                    case ShopData.Player_ParamList.Param_Interval:
                        break;
                    default:
                        break;
                }
                break;
            case BtnGroup.Tower:
                break;
            case BtnGroup.Skill:
                break;
            case BtnGroup.Ult:
                break;
            default:
                break;
        }
    }

}
