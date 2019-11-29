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

    public Subject<int> addLevel_Player = new Subject<int>();
    public Subject<int> addLevel_Tower = new Subject<int>();
    public Subject<int> addLevel_Ult = new Subject<int>();

    protected override void Awake()
    {
        base.Awake();
        shopDataList = Resources.Load<ShopDataList>("ShopDataList");
        shopData = shopDataList.dataList_Shop[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
