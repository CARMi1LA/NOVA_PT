using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAttack : MonoBehaviour
{
    // 通常攻撃

    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    TDList.BulletTypeList bType = TDList.BulletTypeList.Normal;  // 攻撃タイプの実装テスト

    bool isAttack = false;

    void Start()
    {
        pManager.attackTrigger
            .Subscribe(value =>
            {
                isAttack = value;

            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(x => isAttack)
            .ThrottleFirstFrame(pManager.pData.pAttackInterval)
            .Subscribe(_ =>
            {
                // 通常攻撃の実行
                TDBulletData bData = new TDBulletData(pManager.pData.pParent,bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);

            }).AddTo(this.gameObject);
    }
}
