using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    TDEnemyUnit eUnit;
    [SerializeField]
    TDList.BulletTypeList bType = TDList.BulletTypeList.Normal;
    TDEnemyBulletList eBulletList;
    TDEnemyBullet eBullet;
    bool isAttack = false;
    void Awake()
    {
        eBulletList = new TDEnemyBulletList();
        eBullet = eBulletList.GetEnemyBullet(bType);
    }
    void Start()
    {
        Debug.Log(bType.ToString());
        // 攻撃指示を受け取る
        eUnit.eManager.isTargetPlayer
            .Subscribe(value =>
            {
                isAttack = value;

            }).AddTo(this.gameObject);
        // 更新
        this.UpdateAsObservable()
            .Where(x => isAttack)
            .ThrottleFirst(System.TimeSpan.FromSeconds(eBullet.bInterval))
            .Subscribe(_ =>
            {
                eBullet = eBulletList.GetEnemyBullet(bType);

                foreach (var item in eBullet.bDataList)
                {
                    var bul = item;
                    // 自分の現在位置情報を追加
                    bul.bPosition += this.transform.position;
                    bul.bRotation += this.transform.eulerAngles;
                    // 弾を生成
                    TDBulletSpawner.Instance.bulletRentSubject.OnNext(bul);
                }

            }).AddTo(this.gameObject);
    }
}
