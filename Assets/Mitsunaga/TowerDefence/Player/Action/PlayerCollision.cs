using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCollision : MonoBehaviour, IDamageTD, ICollisionTD
{
    // コライダーの管理
    // オブジェクトと衝突したとき、IDamage,ICollisionインターフェイスを取得してそれぞれを起動する
    // また、IDamageとICollisionインターフェイスを継承しそれぞれの処理を行う

    [SerializeField]
    TDPlayerManager pManager;
    
    bool isDash = false;    // ダッシュ中は弾によるダメージを受けない

    void Start()
    {
        // 衝突判定
        this.OnCollisionEnterAsObservable()
            .Subscribe(col =>
            {
                // 相手にダメージを与える
                if (col.gameObject.GetComponent<IDamageTD>() != null)
                {
                    col.gameObject.GetComponent<IDamageTD>().HitDamage(pManager.pData.pParent);
                }
                // 相手をふっとばす
                if (col.gameObject.GetComponent<ICollisionTD>() != null)
                {
                    col.gameObject.GetComponent<ICollisionTD>().HitCollision(this.transform.position);
                }

            }).AddTo(this.gameObject);

        // ダッシュ中の無敵判定
        pManager.DashTrigger
            .Do(_ =>
            {
                isDash = true;
            })
            .Delay(System.TimeSpan.FromSeconds(pManager.pData.pDashTime))
            .Subscribe(_ =>
            {
                isDash = false;

            }).AddTo(this.gameObject);
    }

    // 被ダメージ処理
    public void HitDamage(TDList.ParentList parent)
    {
        // 陣営の確認
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
    // 衝突処理
    public void HitCollision(Vector3 targetPos)
    {
            // 衝突イベント発行
            pManager.ImpactTrigger.OnNext(targetPos);
    }
}
