using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyMove : MonoBehaviour
{
    // 敵キャラクターの移動管理
    [SerializeField]
    TDEnemyManager eManager;
    TDEnemyData eData;

    Rigidbody eRigidbody;

    float eSpeedDownMul;

    float eSpeedNormal  = 1.0f;
    float eSpeedSlow    = 2.0f;

    Vector3 targetPosition;     // ターゲットの位置(タワー、もしくはプレイヤー)
    float rotSpeed = 360.0f;    // 回転速度

    void Awake()
    {
        eRigidbody = this.GetComponent<Rigidbody>();
        targetPosition = eManager.targetPosition;
    }
    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // 方向転換
                {
                    float rotAngle = 0;

                    Vector3 target = targetPosition - this.transform.position;
                    if(Vector3.Cross(target,this.transform.forward).y > 0.01)
                    {
                        rotAngle = -1.0f;
                    }
                    else if (Vector3.Cross(target, this.transform.forward).y < -0.01)
                    {
                        rotAngle = 1.0f;
                    }

                    this.transform.localEulerAngles += new Vector3(0, rotAngle * rotSpeed * Time.deltaTime, 0);
                }

                // 加速
                eRigidbody.AddForce(this.transform.forward * eData.eSpeed * eData.eSpeedMul);
                // 減速　摩擦等
                eRigidbody.AddForce(-eRigidbody.velocity * eData.eSpeedMul * eSpeedDownMul);

            }).AddTo(this.gameObject);

        // プレイヤーをターゲット
        eManager.isTargetPlayer
            .Subscribe(value =>
            {
                if (value)
                {
                    targetPosition = eManager.playerPosition;
                }
                else
                {
                    targetPosition = eManager.targetPosition;
                }

            }).AddTo(this.gameObject);
        // スロートラップを踏んだ
        eManager.SlowTrigger
            .Subscribe(value =>
            {
                if (value)
                {
                    eSpeedDownMul = eSpeedSlow;
                }
                else
                {
                    eSpeedDownMul = eSpeedNormal;
                }

            }).AddTo(this.gameObject);
    }
}
