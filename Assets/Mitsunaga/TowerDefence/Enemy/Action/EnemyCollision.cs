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
                // TowerにぶつかったらそのTowerManagerを取得してダメージを与える
                // もしくはその他の方法でタワーを取得できるのであれば、ターゲットタワーにダメージを与える
                if(col.gameObject.tag == "Tower")
                {
                    eUnit.eManager.TowerHitTrigger.OnNext(eUnit.eManager.targetTsf.gameObject.GetComponent<TowerManager>());
                }

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

        this.OnTriggerEnterAsObservable()
            .Where(x => x.gameObject.tag == "Trap")
            .Subscribe(col =>
            {
                eUnit.eManager.SlowTrigger.Value = true;

            }).AddTo(this.gameObject);

        this.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.tag == "Trap")
            .Subscribe(col =>
            {
                eUnit.eManager.SlowTrigger.Value = false;

            }).AddTo(this.gameObject);
    }

    // ダメージ処理
    public void HitDamage(TDList.ParentList eparent)
    {
        // 陣営の確認
        if (eparent != eUnit.eManager.eParent)
        {
            // ダメージを受ける
            eUnit.DamageTrigger.OnNext(eparent);
        }
    }

    // 衝突処理
    public void HitCollision(Vector3 targetPos)
    {
        if(eUnit.eManager.eData.eSize != TDList.EnemySizeList.Extra)
        {
            // 衝突イベント発行
            eUnit.eManager.ImpactTrigger.OnNext(targetPos);
        }
    }
}
