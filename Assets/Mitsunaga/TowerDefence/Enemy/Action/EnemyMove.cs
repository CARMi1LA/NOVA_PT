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
    [SerializeField]
    float maxRotate = 2.0f;    // 回転速度

    // 何らかのアクション
    bool isAction = false;
    float actionCount = 0.0f;       // 行動不能のカウント
    // 衝突
    float impactSpeed = 100.0f;        // 衝突のふっとび速度
    float impactInterval = 0.2f;    // 衝突の行動不能時間


    void Awake()
    {
        eRigidbody = this.GetComponent<Rigidbody>();
        targetPosition = eManager.targetPosition;
    }
    void Start()
    {
        eData = eManager.eData;

        // アクション時の操作不能時間を計測
        this.UpdateAsObservable()
            .Where(x => isAction)
            .Subscribe(_ =>
            {
                actionCount += -Time.deltaTime;
                if (actionCount <= 0.0f)
                {
                    isAction = false;
                }
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {

                // 方向転換
                {
                    float rotAngle = 0;

                    Vector3 target = targetPosition - this.transform.position;
                    Vector3 cross = Vector3.Cross(target, this.transform.forward);
                    if (cross.y > 0)
                    {
                        rotAngle = -1.0f;
                    }
                    else if (cross.y < 0)
                    {
                        rotAngle = 1.0f;
                    }
                    float angle = Vector3.Angle(this.transform.forward, target) * rotAngle;
                    if(angle > maxRotate)
                    {
                        angle = maxRotate;
                    }
                    else if(angle < -maxRotate)
                    {
                        angle = -maxRotate;
                    }
                    this.transform.localEulerAngles += new Vector3(0, angle, 0);

                    //rotAngle = Vector3.Cross(this.transform.forward, target).y;
                    //this.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(this.transform.localEulerAngles.y,rotAngle,0.1f), 0);
                }

                // 加速
                if(!isAction)
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

        // 衝突
        eManager.ImpactTrigger
            .Subscribe(value =>
            {
                Vector3 dir = (this.transform.position - value).normalized;
                eRigidbody.AddForce(dir * impactSpeed, ForceMode.Impulse);
                // 一定時間行動不能にする
                actionCount = impactInterval;
                isAction = true;

            }).AddTo(this.gameObject);
    }
}
