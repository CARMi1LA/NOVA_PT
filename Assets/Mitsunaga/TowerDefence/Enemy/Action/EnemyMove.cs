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
    float rotSpeed = 50.0f;    // 回転速度

    void Awake()
    {
        eRigidbody = this.GetComponent<Rigidbody>();
        targetPosition = eManager.targetPosition;
    }
    void Start()
    {
        eData = eManager.eData;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // 方向転換
                {
                    float rotAngle = 0;

                    Vector3 target = targetPosition - this.transform.position;
                    //if(Vector3.Cross(target,this.transform.forward).y > 0.1)
                    //{
                    //    rotAngle = -50.0f;
                    //}
                    //else if (Vector3.Cross(target, this.transform.forward).y < -0.1)
                    //{
                    //    rotAngle = 50.0f;
                    //}
                    float angle = -Vector3.Cross(target, this.transform.forward).y;

                    Vector3 targetRot = new Vector3(0, angle * rotSpeed * Time.deltaTime, 0);
                    eRigidbody.AddTorque(targetRot);
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
