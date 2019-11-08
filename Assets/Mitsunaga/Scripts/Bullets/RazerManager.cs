using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class RazerManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] ChildrenPS;

    float playTime = 2.0f;

    /*
    public Subject<RazerData> InitPSSubject = new Subject<RazerData>();
    void Start()
    {
        ParticleSystem thisPS = this.GetComponent<ParticleSystem>();

        InitPSSubject
            .Subscribe(value =>
            {
                thisPS.startLifetime = value.rDelay;

                this.transform.localPosition = value.rPosition;

                Vector3 euler = new Vector3(0.0f, Mathf.Atan2(value.rRotation.x, value.rRotation.z) * Mathf.Rad2Deg, 0.0f);

                foreach (var ps in ChildrenPS)
                {
                    ps.transform.localEulerAngles = euler;
                }

                thisPS.Play();

                Observable
                    .Timer(TimeSpan.FromSeconds(value.rDelay + playTime))
                    .Subscribe(_ =>
                    {
                        thisPS.Stop();

                        RazerSpawner.Instance.razerPool.Return(this);
                    })
                    .AddTo(this.gameObject);
            })
            .AddTo(this.gameObject);
    }*/

    public IObservable<Unit> InitParticle(RazerData razerData)
    {
        ParticleSystem thisPS = this.GetComponent<ParticleSystem>();
        thisPS.startLifetime = razerData.rDelay;

        this.transform.localPosition = razerData.rPosition;

        Vector3 euler = new Vector3(0.0f, Mathf.Atan2(razerData.rRotation.x, razerData.rRotation.z) * Mathf.Rad2Deg, 0.0f);

        foreach (var ps in ChildrenPS)
        {
            ps.transform.localEulerAngles = euler;
        }

        thisPS.Play();

        return Observable
            .Timer(TimeSpan.FromSeconds(razerData.rDelay + playTime))
            .ForEachAsync(_ => thisPS.Stop());
    }
}
