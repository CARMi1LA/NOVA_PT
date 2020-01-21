using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDBulletSpawner : TDBulletSpawnerSingleton<TDBulletSpawner>
{
    [SerializeField]
    TDBulletManager bPrefab;

    TDBulletPool bPool;

    public Subject<TDBulletData>    bulletRentSubject   = new Subject<TDBulletData>();
    public Subject<TDBulletManager> bulletReturnSubject = new Subject<TDBulletManager>();

    protected override void Awake()
    {
        base.Awake();

        bPool = new TDBulletPool(this.transform, bPrefab);
    }
    void Start()
    {
        // Bulletの生成イベント
        bulletRentSubject
            .Subscribe(value =>
            {
                Debug.Log("Bullet生成：" + value.bParent);
                // プールから借りる
                TDBulletManager bManager = bPool.Rent();
                bManager.Init(value);

            }).AddTo(this.gameObject);

        // Bulletの返却イベント
        bulletReturnSubject
            .Subscribe(value =>
            {
                // プールに返す
                bPool.Return(value);

            }).AddTo(this.gameObject);
    }
}
