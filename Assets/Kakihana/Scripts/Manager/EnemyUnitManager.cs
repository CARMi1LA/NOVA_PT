using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyUnitManager : MonoBehaviour
{
    // 敵パターンごとの敵数を管理するクラス

    public EnemyManager[] unitEnemys; // 敵の情報
    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => unitEnemys == null)
            .Subscribe(_ =>
            {
                Destroy(this.gameObject);
            }).AddTo(this.gameObject);
    }
}
