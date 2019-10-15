using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class EnemyUnitManager : MonoBehaviour
{
    // 敵パターンごとの敵数を管理するクラス

    public EnemyManager[] unitEnemys; // 敵の情報
    private void Start()
    {
        this.UpdateAsObservable()
            .Sample(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                int count = transform.childCount;
                if (count == 0)
                {
                    Destroy(this.gameObject);
                }
            }).AddTo(this.gameObject);
    }
}
