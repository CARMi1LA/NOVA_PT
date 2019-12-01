using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCollision : MonoBehaviour, IDamage, ICollision
{
    // コライダーの管理
    
    // 衝突イベント
    public Subject<Vector3> cImpactSubject = new Subject<Vector3>();

    void Start()
    {
        // 衝突判定
        this.OnCollisionEnterAsObservable()
            .Subscribe(col =>
            {
                this.HitCollision(col.transform.position); // デバッグ用のセルフふっとばし

                // エネミーのオブジェクトと衝突した場合
                if (col.gameObject.tag == "Enemy")
                {
                    // 相手にダメージを与える
                    if (col.gameObject.GetComponent<IDamage>() != null)
                    {
                        col.gameObject.GetComponent<IDamage>().HitDamage();
                    }
                    // 相手をふっとばす
                    if (col.gameObject.GetComponent<ICollision>() != null)
                    {
                        col.gameObject.GetComponent<ICollision>().HitCollision(this.transform.position);
                    }
                    Debug.Log("エネミーと衝突した！");
                }

            }).AddTo(this.gameObject);

        // 重複判定
        this.OnTriggerEnterAsObservable()
            .Subscribe(col =>
            {
                // エネミーのオブジェクトと重複した場合
                if (col.gameObject.tag == "Enemy")
                {
                    Debug.Log("エネミーと衝突した！");
                }
            }).AddTo(this.gameObject);
    }

    public void HitDamage()
    {
        // ダメージを受ける
        
    }

    public void HitCollision(Vector3 targetPos)
    {
        // 衝突イベント発行
        cImpactSubject.OnNext(targetPos);
    }
}
