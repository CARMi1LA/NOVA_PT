using System;
using UnityEngine;
using UniRx;

public class EnemyFlash : MonoBehaviour
{
    Renderer enemyRenderer;

    [SerializeField]BoolReactiveProperty isDamage = new BoolReactiveProperty(false);// これいらない

    Subject<float> FlashSubject = new Subject<float>();
    
    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();

        FlashSubject
            .Subscribe(value => 
            {
                enemyRenderer.material.SetInt("_IsDamage", 1);

                Observable.Timer(TimeSpan.FromSeconds(value))
                .Subscribe(_ =>
                {
                    enemyRenderer.material.SetInt("_IsDamage", 0);
                })
                .AddTo(this.gameObject);
            })
            .AddTo(this.gameObject);

        isDamage
            .Where(x => x)
            .Subscribe(_ =>
            {
                FlashSubject.OnNext(2.0f);
            })
            .AddTo(this.gameObject);
    }
}
