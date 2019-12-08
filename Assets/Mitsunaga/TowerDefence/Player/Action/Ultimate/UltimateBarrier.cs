using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class UltimateBarrier : MonoBehaviour
{
    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    float ultTime;
    [SerializeField]
    ParticleSystem ultParticle;

    BoolReactiveProperty isBarrier = new BoolReactiveProperty(false);

    void Start()
    {
        isBarrier.Value = false;

        pManager.ultimateTrigger
            .Subscribe(value =>
            {
                isBarrier.Value = true;

                Observable.Timer(System.TimeSpan.FromSeconds(ultTime))
                .Subscribe(_ =>
                {
                    isBarrier.Value = false;

                }).AddTo(this.gameObject);

            }).AddTo(this.gameObject);

        isBarrier
            .Subscribe(value =>
            {
                pManager.pData.pBarrier = value;

                if (value)
                {
                    ultParticle.Play();
                }
                else
                {
                    ultParticle.Stop();
                }


            }).AddTo(this.gameObject);
    }
}
