using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;

public class ItemPool : ObjectPool<ItemManager>
{

    /* 
        アイテムオブジェクトをプール（キャッシュ）化して管理するスクリプト

        InstantiateとDestroyは負荷が大きいため、
        オブジェクトを初回で一定数生成、プール化して必要でなければ非表示
        必要な時に初期化して表示させると言った動作にし負荷を軽減させる

        敵以外にもオブジェクトを多数生成させるため、
        オブジェクトの種類ごとにプールクラスを分けます
    */
    public readonly ItemManager itemObj;    // 生成アイテム
    public Transform itemObjTrans;          // プールしたオブジェクトをまとめるオブジェクトの座標

    // コンストラクタ
    public ItemPool(ItemManager item, Transform trans)
    {
        itemObj = item;
        itemObjTrans = trans;
    }

    // アイテムを出現させる
    protected override ItemManager CreateInstance()
    {
        var e = GameObject.Instantiate(itemObj);
        e.transform.SetParent(itemObjTrans);

        return e;
    }
}
