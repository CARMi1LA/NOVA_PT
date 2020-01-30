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

    float eSpeedDownMul = 1.0f;

    float eSpeedNormal  = 1.0f;
    float eSpeedSlow    = 2.0f;

    Vector3 targetPosition;     // ターゲットの位置(タワー、もしくはプレイヤー)

    // 何らかのアクション
    bool isAction = false;
    float actionCount = 0.0f;       // 行動不能のカウント
    // 衝突
    float impactSpeed = 100.0f;        // 衝突のふっとび速度
    float impactInterval = 0.2f;    // 衝突の行動不能時間

    void Awake()
    {
        eRigidbody = this.GetComponent<Rigidbody>();

        eManager.InitTrigger
            .Subscribe(value =>
            {
                eData = value;
                targetPosition = eManager.targetTsf.position;

            }).AddTo(this.gameObject);
    }
    void Start()
    {
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
                    if (eManager.isTargetPlayer.Value && eManager.eSize != TDList.EnemySizeList.Extra)
                    {
                        targetPosition = eManager.playerTsf.position;
                    }
                    else
                    {
                        targetPosition = eManager.targetTsf.position;
                    }

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
                    if(angle > eData.eRotSpeed)
                    {
                        angle = eData.eRotSpeed;
                    }
                    else if(angle < -eData.eRotSpeed)
                    {
                        angle = -eData.eRotSpeed;
                    }
                    this.transform.localEulerAngles += new Vector3(0, angle, 0) * Time.deltaTime;

                    //rotAngle = Vector3.Cross(this.transform.forward, target).y;
                    //this.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(this.transform.localEulerAngles.y,rotAngle,0.1f), 0);
                }

                // 加速
                if(!isAction)
                eRigidbody.AddForce(this.transform.forward * eData.eSpeed * eData.eSpeedMul);
                // 減速　摩擦等
                eRigidbody.AddForce(-eRigidbody.velocity * eData.eSpeedMul * eSpeedDownMul);

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

        // タワーに触れたら行動停止、一定時間後に爆発する
        eManager.TowerHitTrigger
            .Do(value =>
            {
                eRigidbody.isKinematic = true;
                Instantiate(eManager.towerHitParticle.gameObject, this.transform.position, Quaternion.identity)
                    .gameObject.transform.parent = this.transform;
            })
            .Delay(System.TimeSpan.FromSeconds(eManager.towerHitInterval))
            .Subscribe(value =>
            {
                Debug.Log("Tower Hit：" + eData.eTowerDamage.ToString());
                value.HitDamage(eData.eTowerDamage);
                eManager.CoreDeathTrigger.OnNext(Unit.Default);
            })
            .AddTo(this.gameObject);
    }
}
