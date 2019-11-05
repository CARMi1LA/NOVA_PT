using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class RazerManager : MonoBehaviour
{
    [SerializeField]
    float waitTime = 1.0f;
    [SerializeField]
    Transform pT;
    [SerializeField]
    RazerObject razerPrefab;

    float maxRayDistance = 50.0f;

    public Subject<RazerData> RazerSubject = new Subject<RazerData>();

    void Start()
    {
        // デバッグ用
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RazerData rd = new RazerData(RazerData.RazerParent.Player, waitTime, 1.0f, pT.localPosition, pT.localEulerAngles);

                    RazerSubject.OnNext(rd);

                }
                float RadY = pT.localEulerAngles.y * Mathf.Deg2Rad;
                Vector3 rVec = new Vector3(Mathf.Sin(RadY), 0.0f, Mathf.Cos(RadY)).normalized;

                Debug.DrawRay(pT.localPosition, rVec* maxRayDistance, Color.white);
            })
            .AddTo(this.gameObject);

        RazerSubject
            .Subscribe(value =>
            {
                RazerObject rp = Instantiate(razerPrefab);
                rp.InitParticle(value.rDelay, value.rPosition, value.rRotation);

                // ディレイの長さを可変にしたいが長さをRazerDataに格納すると.Delay()が使えないためTimerを用いる
                Observable
                .Timer(System.TimeSpan.FromSeconds(value.rDelay))
                .Subscribe(_ =>
                {
                    float RadY = value.rRotation.y * Mathf.Deg2Rad;
                    Vector3 rVec = new Vector3(Mathf.Sin(RadY), 0.0f, Mathf.Cos(RadY)).normalized;

                    // RaycastAll(Ray(始点,方向), float(距離))
                    // ・主にForeach内で使い、それぞれ対象をRaycastHit型に入れて処理を行う
                    Ray r = new Ray(value.rPosition, rVec);

                    foreach (RaycastHit hit in Physics.SphereCastAll(r, value.rRadius, maxRayDistance))
                    {
                        // タグがターゲットか否かを判断する
                        if(hit.transform.tag == ((value.rParent == RazerData.RazerParent.Player)? "Enemy" : "Player"))
                        {
                            // RaycastHit.collider.gameObject で触れたオブジェクトの情報を取り出せる
                            hit.collider.gameObject.GetComponent<IDamage>().HitDamage();
                        }
                    }
                })
                .AddTo(this.gameObject);
            })
            .AddTo(this.gameObject);
    }
}
