using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BulletSpawner : BSSingleton<BulletSpawner>
{
    // 弾生成管理クラス

    [SerializeField] private int bulletValueMax;            // 弾の最大生成数
    [SerializeField] public IntReactiveProperty bulletCount = new IntReactiveProperty(0); // 弾の生成数
    [SerializeField] private BulletManager[] bulletObj;     // 弾のプレハブ
    [SerializeField] private BulletPool bulletPool;         // 弾のオブジェクトプールクラス

    [SerializeField] private Transform bulletPoolTrans;     // オブジェクトプールを格納するための変数

    // 生成予定の弾のデータを格納するリスト
    [SerializeField] public ReactiveCollection<BulletData> bulletDataList = new ReactiveCollection<BulletData>();

    // 生成した弾を格納するリスト
    [SerializeField] public ReactiveCollection<BulletManager> bulletList = new ReactiveCollection<BulletManager>();
    // Start is called before the first frame update
    void Start()
    {
        // プールの初期化
        bulletPool = new BulletPool(bulletObj[0], bulletPoolTrans);

        // 弾生成処理、生成予定のデータリストに情報が追加された時に動作
        bulletDataList.ObserveAdd()
        .Where(_ => bulletCount.Value <= bulletValueMax)
        .Subscribe(_ =>
        {
            // プールの生成
            var bullet = bulletPool.Rent();
            // 各弾を生成
            bullet.BulletCreate(_.Value);
            // 生成済みリスト情報を追加
            bulletList.Add(bullet);
            // 弾生成用データリストは不要になるので破棄する。
            bulletDataList.Remove(_.Value);
        }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => bulletDataList.Count >= bulletValueMax * 0.75f).Subscribe(_ =>
            {
                bulletDataList.Clear();
            }).AddTo(this.gameObject);
    }

    // プールの返却と出現済みリストから弾の情報を削除するメソッド
    public void BulletRemove(BulletManager bm)
    {
        bulletPool.Return(bm);
        bulletList.Remove(bm);
    }
}

