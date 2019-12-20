using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCollision : MonoBehaviour, IDamageTD, ICollisionTD
{
    // コライダーの管理
    [SerializeField]
    TDPlayerManager pManager;
    
    // ダッシュ中は弾によるダメージを受けない
    bool isDash = false;

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => Input.GetKeyDown(KeyCode.H))
            .Subscribe(_ =>
            {
                HitDamage(pManager.pData.pParent);

            }).AddTo(this.gameObject);

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
                    if (col.gameObject.GetComponent<ICollisionTD>() != null)
                    {
                        col.gameObject.GetComponent<ICollisionTD>().HitCollision(this.transform.position);
                    }
                    Debug.Log("エネミーと衝突した！");
                }

            }).AddTo(this.gameObject);

        // 重複判定
        this.OnTriggerEnterAsObservable()
            .Subscribe(col =>
            {
                // エネミーのオブジェクトと重複した場合
                if (!isDash && col.gameObject.tag == "Enemy")
                {
                    Debug.Log("エネミーと衝突した！");
                }

            }).AddTo(this.gameObject);

        // ダッシュ中の無敵判定
        pManager.dashTrigger
            .Do(_ => isDash = true)
            .Delay(System.TimeSpan.FromSeconds(pManager.pData.pDashTime))
            .Subscribe(_ =>
            {
                isDash = false;

            }).AddTo(this.gameObject);
    }

    public void HitDamage(TDList.ParentList parent)
    {
        if(parent != pManager.pData.pParent)
        {
            // ダメージ無効時間の確認
            if (pManager.pData.pBarrier || isDash)
            {
                Debug.Log("ダメージ無効！");
            }
            else
            {
                // ダメージを受ける
                Debug.Log("ダメージを受けた！");
                pManager.DamageTrigger.OnNext(Unit.Default);
            }
        }
    }

    public void HitCollision(Vector3 targetPos)
    {
        // 衝突イベント発行
        pManager.impactTrigger.OnNext(targetPos);
    }
}
