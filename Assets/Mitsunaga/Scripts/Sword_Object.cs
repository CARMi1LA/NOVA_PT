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

    ReactiveProperty<float> alphaRP = new ReactiveProperty<float>();

    void Start()
    {
        sRot.SwordSubject
            .Subscribe(value =>
            {
                StartCoroutine(ChangeAlphaCoroutine(value, generateTime));
            })
            .AddTo(this.gameObject);

        alphaRP
            .Subscribe(value =>
            {
                Debug.Log(alphaRP.Value.ToString());
            })
            .AddTo(this.gameObject);
    }

    IEnumerator ChangeAlphaCoroutine(float alpha, float time)
    {
        float t = 0.0f;
        float startAlpha = alphaRP.Value;

        while(t < time)
        {
            t += Time.deltaTime;

            alphaRP.Value = Mathf.Lerp(startAlpha, alpha, t / time);

            yield return null;
        }

        alphaRP.Value = alpha;
    }
}
