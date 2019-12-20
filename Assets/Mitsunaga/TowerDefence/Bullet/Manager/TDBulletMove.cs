using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDBulletMove : MonoBehaviour
{
    // Transform・Rigidbodyの管理

    [SerializeField]
    TDBulletManager bManager;

    Rigidbody bRig;
    float bSpeed;

    void Awake()
    {
        // 初期設定
        bRig = this.GetComponent<Rigidbody>();

        // 弾生成後の初期設定
        bManager.initTrigger
            .Subscribe(value =>
            {
                // Velocityを初期化する
                bRig.velocity = Vector3.zero;
                // 初期位置、角度設定
                this.transform.position = value.bPosition;
                this.transform.eulerAngles = value.bRotation;
                // 速度設定
                bSpeed = value.bSpeed;
                // 弾の個性設定
                bulletMove(value.bType);

            }).AddTo(this.gameObject);
    }

    void Start()
    {
        // 更新処理
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // 前方に移動
                bRig.velocity = bSpeed * transform.forward;

            }).AddTo(this.gameObject);
    }


    void bulletMove(TDList.BulletTypeList type)
    {
        switch (type)
        {
            case TDList.BulletTypeList.Normal:
                break;
            default:
                break;
        }
    }
}
