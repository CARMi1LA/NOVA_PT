using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HUD_SearchEnemyMark : MonoBehaviour
{
    // 一定時間後に消滅するオブジェクト

    [SerializeField, Header("自動消滅の時間")]
    float deathCount = 1.0f;

    void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(deathCount))
            .Subscribe(_ =>
            {
                Destroy(this.gameObject);
            })
            .AddTo(this.gameObject);
    }
}
