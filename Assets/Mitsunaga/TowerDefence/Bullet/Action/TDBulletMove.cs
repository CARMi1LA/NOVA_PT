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
                bSpeed = bManager.bForm.bSpeed;
                // 弾の個性設定
                bulletMove(value.bType);

            }).AddTo(this.gameObject);
    }

    void Start()
    {
        // ミサイル用の変数
        Transform targetEnemy = null;
        float rotAngle = 0;
        float rotSpeed = 0.1f;

        // 更新処理
        this.UpdateAsObservable()
            .Where(x => !GameManagement.Instance.isPause.Value)
            .Subscribe(_ =>
            {
                if(targetEnemy != null && bManager.bData.bType == TDList.BulletTypeList.Missile)
                {
                    Vector3 target = targetEnemy.position - this.transform.position;
                    Vector3 cross = Vector3.Cross(target, this.transform.forward);
                    if (cross.y > 0)
                    {
                        rotAngle = -rotSpeed;
                    }
                    else if (cross.y < 0)
                    {
                        rotAngle = rotSpeed;
                    }
                    float angle = Vector3.Angle(this.transform.forward, target) * rotAngle;
                    this.transform.localEulerAngles += new Vector3(0, angle, 0);
                }
                // 前方に移動
                bRig.velocity = bSpeed * transform.forward;

            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(x => bManager.bData.bType == TDList.BulletTypeList.Missile)
            .Where(x => !GameManagement.Instance.isPause.Value)
            .Sample(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                float dis = 1000000.0f;
                // Missileは一番近いEnemyに方向転換する
                foreach(var item in GameManagement.Instance.enemyInfoList.enemyInfo)
                {
                    if(item != null)
                    {
                        float itemDis = (this.transform.position - item.position).sqrMagnitude;
                        if(itemDis <= dis)
                        {
                            dis = itemDis;
                            targetEnemy = item;
                        }
                    }
                }

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
