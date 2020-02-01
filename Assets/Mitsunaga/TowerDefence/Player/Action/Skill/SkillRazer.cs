using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillRazer : MonoBehaviour
{
    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    ParticleSystem razerPrefab;
    [SerializeField]
    float razerDelay = 0.5f;
    [SerializeField]
    float razerRadius = 5.0f;
    [SerializeField]
    float razerLength = 800.0f;
    [SerializeField]
    TDList.ParentList razerParent = TDList.ParentList.Player;

    float razerHitDelay = 1.0f;

    void Start()
    {
        ParticleSystem razer = new ParticleSystem();

        pManager.SkillTrigger
            .Where(x => pManager.pData.pSkillType == TDPlayerData.SkillTypeList.Razer)
            .Do(_ =>
            {
                razer = Instantiate(razerPrefab.gameObject).GetComponent<ParticleSystem>();
                razer.gameObject.transform.position = this.transform.position;
                razer.gameObject.transform.eulerAngles = this.transform.eulerAngles;
                razer.gameObject.transform.GetChild(0).localEulerAngles = this.transform.eulerAngles;
                razer.gameObject.transform.GetChild(1).localEulerAngles = this.transform.eulerAngles;
                razer.startLifetime = razerDelay;
                razer.Play();
            })
            .Delay(System.TimeSpan.FromSeconds(razerDelay * 1.2f))
            .Do(_ =>
            {
                Ray r = new Ray(razer.transform.position, razer.transform.forward);

                foreach (RaycastHit hit in Physics.SphereCastAll(r, razerRadius, razerLength))
                {
                    // タグがターゲットか否かを判断する
                    if (hit.collider.gameObject.GetComponent<IDamageTD>() != null)
                    {
                        // RaycastHit.collider.gameObject で触れたオブジェクトの情報を取り出せる
                        hit.collider.gameObject.GetComponent<IDamageTD>().HitDamage(razerParent);
                        hit.collider.gameObject.GetComponent<IDamageTD>().HitDamage(razerParent);
                    }
                }
            })            
            .Delay(System.TimeSpan.FromSeconds(razerHitDelay))
            .Subscribe(_ =>
            {
                Ray r = new Ray(razer.transform.position, razer.transform.forward);

                foreach (RaycastHit hit in Physics.SphereCastAll(r, razerRadius, razerLength))
                {
                    // タグがターゲットか否かを判断する
                    if (hit.collider.gameObject.GetComponent<IDamageTD>() != null)
                    {
                        // RaycastHit.collider.gameObject で触れたオブジェクトの情報を取り出せる
                        hit.collider.gameObject.GetComponent<IDamageTD>().HitDamage(razerParent);
                        hit.collider.gameObject.GetComponent<IDamageTD>().HitDamage(razerParent);
                    }
                }
            })
            .AddTo(this.gameObject);
    }
}
