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
        pManager.ultimateTrigger
            .Subscribe(value =>
            {
                // アルティメット実行時の共通行動(エフェクトなど)
                ultParticle.Play();
                Observable.TimerFrame(20)
                    .Subscribe(_ => ultParticle.Stop())
                    .AddTo(this.gameObject);

            }).AddTo(this.gameObject);
    }
}
