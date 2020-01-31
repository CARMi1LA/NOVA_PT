using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDBulletCollision : MonoBehaviour
{
    // 弾の当たり判定の管理

    [SerializeField]
    TDBulletManager bManager;

    void Start()
    {
        // 重複判定
        this.OnTriggerEnterAsObservable()
            .Subscribe(value =>
            {
                // 重複オブジェクトに衝突判定があるか
                if (value.isTrigger == false)
                {
                    // ダメージ判定があるか
                    if (value.GetComponent<IDamageTD>() != null)
                    {
                        value.GetComponent<IDamageTD>().HitDamage(bManager.bData.bParent);
                    }
                    // この弾を消滅させる
                    if(bManager.bForm.bType != TDList.BulletTypeList.Bash)
                    {
                        bManager.isReturn.Value = true;
                    }
                }

            }).AddTo(this.gameObject);
    }
}
