using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class ShopBtnManager : MonoBehaviour
{

    public ShopData shopData;

    public Button myBtn;
    public Text productName;        // 商品名テキスト
    public Text levelText;          // 現在の商品ごとのレベル
    public Text materValueText;     // 購入価格

    // Start is called before the first frame update
    void Start()
    {
        shopData = ShopManager.Instance.shopData;
    }

}
