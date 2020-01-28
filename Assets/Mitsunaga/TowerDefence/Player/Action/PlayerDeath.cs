using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerDeath : MonoBehaviour
{
    // プレイヤーの死亡演出
    [SerializeField]
    TDPlayerManager pManager;

    [SerializeField] ParticleSystem deathParticle;    // エフェクト

    void Start()
    {
        pManager.isDeath
            .Where(x => x)
            .Subscribe(value =>
            {
                deathParticle.Play();
                Observable.Timer(System.TimeSpan.FromSeconds(deathParticle.main.duration))
                    .Subscribe(_ => deathParticle.Stop())
                    .AddTo(this.gameObject);

                Observable.Timer(System.TimeSpan.FromSeconds(3.0f))
                    .Subscribe(_ =>
                    {

                    }).AddTo(this.gameObject);

            }).AddTo(this.gameObject);
    }

    // 死亡演出
    public void ActionDeath()
    {
        // エフェクト再生
        deathParticle.Play();
    }
}
