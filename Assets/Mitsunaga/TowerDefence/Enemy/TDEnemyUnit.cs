﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class TDEnemyUnit : MonoBehaviour
{
    // エネミーのユニットの管理
    [SerializeField]
    public TDEnemyManager eManager;

    [SerializeField]
    bool isCoreUnit = false;

    // 死亡時のエフェクト
    public ParticleSystem deathParticle;

    // ヘルス
    public IntReactiveProperty eHealth = new IntReactiveProperty(100);

    // ダメージ処理
    public Subject<TDList.ParentList> DamageTrigger = new Subject<TDList.ParentList>();

    // 死亡処理
    public Subject<Unit> DeathTrigger = new Subject<Unit>();
    private void Awake()
    {
        // ユニットの初期化
        eManager.InitTrigger
            .Do(value =>        // 通常ユニットの場合
            {
                eHealth.Value = value.eHealth;
            })
            .Where(x => isCoreUnit)
            .Subscribe(value => // コアユニットの場合
            {
                eHealth.Value = value.eCoreHealth;
            })
            .AddTo(this.gameObject);

        if(eManager.eSize == TDList.EnemySizeList.Extra && isCoreUnit)
        {
            GameManagement.Instance.bossSpawn.OnNext(this);
        }
    }
    void Start()
    {
        // ヘルスが0になった時の処理
        this.UpdateAsObservable()
            .Where(x => eHealth.Value <= 0)
            .Subscribe(_ =>
            {
                if (isCoreUnit)
                {
                    eManager.CoreDeathTrigger.OnNext(Unit.Default);
                    DeathTrigger.OnNext(Unit.Default);
                }
                else
                {
                    DeathTrigger.OnNext(Unit.Default);
                }

            }).AddTo(this.gameObject);

        // 直死
        eManager.CoreDeathTrigger
            .Subscribe(_ =>
            {
                DeathTrigger.OnNext(Unit.Default);

            }).AddTo(this.gameObject);

        eManager.CoreDeathTrigger
            .Where(x => eManager.eData.eSize == TDList.EnemySizeList.Extra)
            .Subscribe(_ =>
            {
                GameManagement.Instance.tdGameClear.Value = true;
            }).AddTo(this.gameObject);
    }
}
