using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class Sword_Object : MonoBehaviour
{
    [SerializeField]
    Sword_Rotation sRot;
    [SerializeField]
    float generateTime;
    [SerializeField]
    ParticleSystem psSword;

    ReactiveProperty<float> alphaRP = new ReactiveProperty<float>();

    Renderer sRenderer;
    Collider sCollider;

    void Start()
    {
        sRenderer = GetComponent<Renderer>();
        sCollider = GetComponent<Collider>();
        sCollider.enabled = false;

        sRot.SwordGenerateRP
            .Subscribe(value =>
            {
                Debug.Log("SwordGenerateRP " + value);
                StartCoroutine(ChangeAlphaCoroutine(value, generateTime));
            })
            .AddTo(this.gameObject);

        alphaRP
            .Subscribe(value =>
            {
                sRenderer.material.SetFloat("_Threshold", value);
            })
            .AddTo(this.gameObject);
    }

    IEnumerator ChangeAlphaCoroutine(bool value, float time)
    {
        psSword.Stop();
        sCollider.enabled = false;

        float t = 0.0f;
        float a = (value) ? 1 : 0;
        float startAlpha = alphaRP.Value;

        while(t < time)
        {
            t += Time.deltaTime;

            alphaRP.Value = Mathf.Lerp(startAlpha, a, t / time);

            yield return null;
        }

        if(value)
        {
            psSword.Play();
            sCollider.enabled = true;
        }

        alphaRP.Value = a;
    }
}
