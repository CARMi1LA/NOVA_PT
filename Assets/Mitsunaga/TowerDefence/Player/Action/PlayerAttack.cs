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
    TDBulletData.BulletTypeList bType;  // 攻撃タイプの実装テスト
    [SerializeField]
    int attackInterval = 1;   // 攻撃間隔の倍率　実装テスト

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
            .ThrottleFirstFrame(pManager.pData.pAttackInterval * attackInterval)
            .Subscribe(_ =>
            {
                // 通常攻撃の実行
                Debug.Log("通常攻撃　実行");
                TDBulletData bData = new TDBulletData(0,bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);

            }).AddTo(this.gameObject);
    }
}
