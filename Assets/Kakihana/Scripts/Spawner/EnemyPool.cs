using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;

public class EnemyPool : ObjectPool<EnemyManager>
{
    /* 
         敵のオブジェクトをプール（キャッシュ）化して管理するスクリプト

         InstantiateとDestroyは負荷が大きいため、
         オブジェクトを初回で一定数生成、プール化して必要でなければ非表示
         必要な時に初期化して表示させると言った動作にし負荷を軽減させる
       
         敵以外にもオブジェクトを多数生成させるため、
         現在のプロジェクトではオブジェクトの種類ごとにプールクラスを分けます
    */

    public readonly EnemyManager enemyObj;       // プールしたい敵オブジェクト
    private Transform myTrans;                   // プールしたオブジェクトをまとめるオブジェクトの座標

    // コンストラクタ
    public EnemyPool(Transform trans, EnemyManager enemy)
    {
        myTrans = trans;
        enemyObj = enemy;
    }

    // 敵をスポーンさせる
    protected override EnemyManager CreateInstance()
    {
        var e = GameObject.Instantiate(enemyObj);
        e.transform.SetParent(myTrans);

        return e;
    }
}
