using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCollision : MonoBehaviour, IDamage, ICollision
{
    public Subject<int> cDamageSubject = new Subject<int>();

    void Start()
    {
        // 衝突判定
        this.OnCollisionEnterAsObservable()
            .Subscribe(col =>
            {
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
        cDamageSubject.OnNext(1);
    }

    public void HitCollision(Vector3 targetPos)
    {
        // targetPosに弾かれるように移動する
    }
}
