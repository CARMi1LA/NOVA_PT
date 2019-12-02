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
            .ThrottleFirst(System.TimeSpan.FromSeconds(pManager.pData.pAttackInterval))
            .Subscribe(_ =>
            {
                // 通常攻撃の実行
                Debug.Log("通常攻撃　実行");

            }).AddTo(this.gameObject);
    }
}
