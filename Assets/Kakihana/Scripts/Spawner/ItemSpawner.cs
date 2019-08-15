using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ItemSpawner : ISSingleton<ItemSpawner>
{
    // アイテム生成管理クラス（中身は弾スポナーと大体同じ）

    [SerializeField] private int itemSpawnCountMax;     // アイテムの最大生成数
    [SerializeField] public int itemSpawnCount;         // 現在のアイテム生成数

    [SerializeField] ItemManager[] itemObj;             // 出現させたいアイテムのオブジェクト
    [SerializeField] ItemPool itemPool;                 // アイテム用オブジェクトプール
    [SerializeField] private Transform itemPoolTrans;   // オブジェクトプールをまとめる座標

    // 出現予定のデータをまとめるリスト
    public ReactiveCollection<ItemData> itemDataList = new ReactiveCollection<ItemData>();
    // 出現後のアイテムをまとめるリスト
    public ReactiveCollection<ItemManager> itemSpawnList = new ReactiveCollection<ItemManager>();
    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトプールの初期化
        itemPool = new ItemPool(itemObj[0], itemPoolTrans);

        // 出現予定データリストにアイテムが追加されると実行
        itemDataList.ObserveAdd()
            .Where(_ => itemSpawnCount <= itemSpawnCountMax)
            .Subscribe(_ =>
            {
                // オブジェクトプール化
                var item = itemPool.Rent();
                // アイテムを生成
                item.CreateItem(_.Value.initScore, _.Value.initHp, _.Value.initEnergy, _.Value.initType, _.Value.pos);
                // 生成済みリストに追加
                itemSpawnList.Add(item);
                // データリストから現在のデータを削除
                itemDataList.Remove(_.Value);
            }).AddTo(this.gameObject);
    }
    // アイテムが消滅した時に実行
    public void ItemRemove(ItemManager item)
    {
        // オブジェクトプールの返却
        itemPool.Return(item);
        // 生成済みリストから該当アイテムを削除
        itemSpawnList.Remove(item);
    }
}
public class ItemData
{
    // 生成したいアイテムのデータを格納するクラス
    // 生成したい場合はこのクラスのコンストラクタに値を入れる

    public int initScore;       // スコアのデータ
    public int initHp;          // Hpのデータ
    public int initEnergy;      // エネルギーのデータ

    public ItemManager.ItemType initType;   // アイテムの種類
    public Vector3 pos;

    // コンストラクタ
    public ItemData(int score, int hp, int energy, ItemManager.ItemType type, Vector3 enemyPos)
    {
        // 各種データを設定
        initScore = score;
        initHp = hp;
        initEnergy = energy;
        initType = type;
        pos = enemyPos;
        // 生成予定データリストにこのデータを追加
        ItemSpawner.Instance.itemDataList.Add(this);
    }
}