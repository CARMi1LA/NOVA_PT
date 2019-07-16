using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;

public class BulletPool : ObjectPool<BulletManager>
{
    /* 
        弾のオブジェクトをプール（キャッシュ）化して管理するスクリプト

        InstantiateとDestroyは負荷が大きいため、
        オブジェクトを初回で一定数生成、プール化して必要でなければ非表示
        必要な時に初期化して表示させると言った動作にし負荷を軽減させる

        敵以外にもオブジェクトを多数生成させるため、
        現在のプロジェクトではオブジェクトの種類ごとにプールクラスを分けます
    */

    public readonly BulletManager bulletObj;
    private Transform myTrans;

    public BulletPool(BulletManager bm, Transform trans)
    {
        bulletObj = bm;
        myTrans = trans;
    }

    protected override BulletManager CreateInstance()
    {
        var e = GameObject.Instantiate(bulletObj);
        e.transform.SetParent(myTrans);

        return e;
    }
}
