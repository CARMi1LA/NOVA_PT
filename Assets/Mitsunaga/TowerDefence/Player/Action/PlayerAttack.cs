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
    [SerializeField]
    int ActiveLevel = 0;

    bool isAttack = false;

    void Start()
    {
        float shotTime = 0.0f;

        pManager.AttackTrigger
            .Subscribe(value =>
            {
                isAttack = value;

            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(x => isAttack)
            .Where(x => !pManager.isDeath.Value)
            .Where(x => ActiveLevel <= ShopManager.Instance.spLv.playerLv.lv_Int.Value)
            .Subscribe(_ =>
            {
                shotTime += Time.deltaTime;

                if(shotTime >= pManager.pData.pAttackInterval)
                {
                    // 通常攻撃の実行
                    TDBulletData bData = new TDBulletData(pManager.pData.pParent, bType, this.transform.position, this.transform.eulerAngles);
                    TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);

                    shotTime = 0.0f;
                }

            }).AddTo(this.gameObject);
    }
}
