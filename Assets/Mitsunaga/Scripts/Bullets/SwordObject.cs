using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class SwordObject : MonoBehaviour
{
    [SerializeField]
    SwordManager sRot;
    [SerializeField]
    float generateTime;
    [SerializeField]
    ParticleSystem psSword;

    ReactiveProperty<float> alphaRP = new ReactiveProperty<float>();

    Renderer sRenderer;
    Collider sCollider;

    void Start()
    {
        sRenderer = GetComponent<Renderer>();   // マテリアルの取得
        sCollider = GetComponent<Collider>();   // 当たり判定の取得
        sCollider.enabled = false;              // 当たり判定を

        // 剣生成イベントの購読
        sRot.SwordGenerateRP
            .Subscribe(value =>
            {
                StartCoroutine(ChangeAlphaCoroutine(value, generateTime));
            })
            .AddTo(this.gameObject);
        // アルファ値の適用
        alphaRP
            .Subscribe(value =>
            {
                sRenderer.material.SetFloat("_Threshold", value);
            })
            .AddTo(this.gameObject);

        // 当たり判定
        this.OnTriggerEnterAsObservable()
            .Subscribe(value =>
            {
                // 重なったオブジェクトのタグが"Enemy"であれば
                if(value.gameObject.tag == "Enemy")
                {
                    // ダメージを与える
                    value.gameObject.GetComponent<IDamage>().HitDamage();
                }
            })
            .AddTo(this.gameObject);
    }

    // 剣のアルファ値変更コルーチン
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
