using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class SkillSwordObject : MonoBehaviour
{
    // 剣の見た目を変更する

    [SerializeField]
    SkillSword sSword;          // 旋風剣
    [SerializeField]
    float generateTime = 0.3f;         // 剣の生成時間
    [SerializeField]
    ParticleSystem sParticle;   // パーティクルの再生

    ReactiveProperty<float> alphaTrigger = new ReactiveProperty<float>();   // アルファ値の変更用

    Renderer sRenderer; // マテリアルの変更用
    Collider sCollider; // 当たり判定の変更用

    void Awake()
    {
        sRenderer = GetComponent<Renderer>();   // マテリアルの取得
        sCollider = GetComponent<Collider>();   // 当たり判定の取得
        sCollider.enabled = false;              // 当たり判定を消しておく
    }

    void Start()
    {
        // 剣生成イベントの購読
        sSword.SwordGenerateTrigger
            .Subscribe(value =>
            {
                StartCoroutine(ChangeAlphaCoroutine(value, generateTime));
            })
            .AddTo(this.gameObject);

        // アルファ値の適用
        alphaTrigger
            .Subscribe(value =>
            {
                sRenderer.material.SetFloat("_Threshold", value);
            })
            .AddTo(this.gameObject);

        // 当たり判定
        this.OnTriggerEnterAsObservable()
            .Subscribe(value =>
            {
                if (value.gameObject.GetComponent<IDamageTD>() != null)
                {
                    // ダメージを与える
                    value.gameObject.GetComponent<IDamageTD>().HitDamage(TDList.ParentList.Player);
                    value.gameObject.GetComponent<IDamageTD>().HitDamage(TDList.ParentList.Player);
                }
            }).AddTo(this.gameObject);
    }

    // 剣のアルファ値変更コルーチン
    IEnumerator ChangeAlphaCoroutine(bool value, float time)
    {
        sParticle.Stop();
        sCollider.enabled = false;

        float t = 0.0f;
        float a = (value) ? 1 : 0;
        float startAlpha = alphaTrigger.Value;

        while(t < time)
        {
            t += Time.deltaTime;

            alphaTrigger.Value = Mathf.Lerp(startAlpha, a, t / time);

            yield return null;
        }

        if(value)
        {
            sParticle.Play();
            sCollider.enabled = true;
        }

        alphaTrigger.Value = a;
    }
}
