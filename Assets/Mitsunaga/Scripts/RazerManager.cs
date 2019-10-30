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

    float maxRayDistance = 50.0f;

    public Subject<RazerData> RazerSubject = new Subject<RazerData>();


    void Start()
    {
        // デバッグ用
        this.UpdateAsObservable()
            .Where(x => Input.GetKeyDown(KeyCode.R))
            .Subscribe(_ =>
            {
                RazerData rd = new RazerData(RazerData.RazerParent.Player, 0.5f, pT.localPosition, pT.localEulerAngles);

                RazerSubject.OnNext(rd);
            })
            .AddTo(this.gameObject);

        RazerSubject
            .Subscribe(value =>
            {
                // ディレイの長さを可変にしたいが長さをRazerDataに格納すると.Delay()が使えないためTimerを用いる
                Observable
                .Timer(System.TimeSpan.FromSeconds(value.rDelay))
                .Subscribe(_ =>
                {
                    // RaycastAll(Ray(始点,方向), float(距離))
                    // ・主にForeach内で使い、それぞれ対象をRaycastHit型に入れて処理を行う
                    Ray r = new Ray(value.rPosition, value.rRotation);
                    foreach(RaycastHit hit in Physics.RaycastAll(r, maxRayDistance))
                    {
                        // タグがターゲットか否かを判断する
                        if(hit.transform.tag == ((value.rParent == RazerData.RazerParent.Player)? "Player" : "Enemy"))
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
