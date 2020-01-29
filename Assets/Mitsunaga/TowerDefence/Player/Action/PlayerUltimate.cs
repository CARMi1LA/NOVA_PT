using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerUltimate : MonoBehaviour
{
    // アルティメットマネージャー

    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    ParticleSystem ultParticle;
    
    void Start()
    {
        pManager.UltimateTrigger
            .Subscribe(value =>
            {
                // アルティメット実行時の共通行動(エフェクトなど)
                ultParticle.Play();
                Observable.Timer(System.TimeSpan.FromSeconds(ultParticle.main.duration))
                    .Subscribe(_ => ultParticle.Stop())
                    .AddTo(this.gameObject);

            }).AddTo(this.gameObject);
    }
}
