using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class RazerSpawner : RazerSpawnerSingleton<RazerSpawner>
{
    [SerializeField]
    RazerManager razerPrefab;

    float maxRayDistance = 50.0f;

    public Subject<RazerData> RazerSubject = new Subject<RazerData>();

    RazerPool razerPool;   // レーザーのオブジェクトプール

    protected override void Awake()
    {
        base.Awake();

        // オブジェクトプールを生成
        razerPool = new RazerPool(this.transform, razerPrefab);
        this.OnDestroyAsObservable().Subscribe(_ => razerPool.Dispose());

    }
    void Start()
    {
        RazerSubject
            .Subscribe(value =>
            {
                // プールから生成、破棄を行う
                RazerManager rm = razerPool.Rent();
                rm.InitParticle(value)
                  .Subscribe(_ =>
                  {
                      razerPool.Return(rm);
                  })
                  .AddTo(this.gameObject);

                // ディレイの長さを可変にしたいが長さをRazerDataに格納すると.Delay()が使えないためTimerを用いる
                Observable
                .Timer(System.TimeSpan.FromSeconds(value.rDelay))
                .Subscribe(_ =>
                {
                    // RaycastAll(Ray(始点,方向), float(距離))
                    // ・主にForeach内で使い、それぞれ対象をRaycastHit型に入れて処理を行う
                    Ray r = new Ray(value.rPosition, value.rRotation);

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
