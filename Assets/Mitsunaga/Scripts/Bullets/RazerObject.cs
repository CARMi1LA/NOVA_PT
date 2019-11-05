using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerObject : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] ChildrenPS;

    public void InitParticle(float waitTime, Vector3 position, Vector3 euler)
    {
        ParticleSystem thisPS = this.GetComponent<ParticleSystem>();
        thisPS.startLifetime = waitTime;

        this.transform.localPosition = position;

        foreach(var ps in ChildrenPS)
        {
            ps.transform.localEulerAngles = euler;
        }

        thisPS.Play();
    }
}
