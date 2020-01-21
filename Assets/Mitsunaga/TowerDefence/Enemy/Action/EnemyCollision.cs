using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyCollision : MonoBehaviour, IDamageTD, ICollisionTD
{
    // コライダーの管理
    // オブジェクトと衝突したとき、IDamage,ICollisionインターフェイスを取得してそれぞれを起動する
    // また、IDamageとICollisionインターフェイスを継承しそれぞれの処理を行う

    [SerializeField]
    TDEnemyUnit eUnit;

    void Start()
    {
        // 衝突判定
        this.OnCollisionEnterAsObservable()
            .Subscribe(col =>
            {
                // 相手にダメージを与える
                if (col.gameObject.GetComponent<IDamageTD>() != null)
                {
                    col.gameObject.GetComponent<IDamageTD>().HitDamage(eUnit.eManager.eParent);
                }
                // 相手をふっとばす
                if (col.gameObject.GetComponent<ICollisionTD>() != null)
                {
                    col.gameObject.GetComponent<ICollisionTD>().HitCollision(this.transform.position);
                }
            }).AddTo(this.gameObject);
    }

    // ダメージ処理
    public void HitDamage(TDList.ParentList eparent)
    {
        // 陣営の確認
        if (eparent != eUnit.eManager.eParent)
        {
            // ダメージを受ける
            Debug.Log("ダメージを受けた！");
            eUnit.DamageTrigger.OnNext(eparent);
        }
    }

    // 衝突処理
    public void HitCollision(Vector3 targetPos)
    {
        // 衝突イベント発行
        eUnit.eManager.ImpactTrigger.OnNext(targetPos);
    }
}
