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
