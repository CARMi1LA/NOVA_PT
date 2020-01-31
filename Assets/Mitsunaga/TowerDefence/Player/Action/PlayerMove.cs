using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMove : MonoBehaviour
{
    // 右スティックと左スティックの値を受け取り、Rigidbodyに力を加える(Addforce)
    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    Transform pRespawnPoint;

    Rigidbody pRigidbody;

    float rotSpeed = 240.0f;        // 回転速度

    // 何らかのアクション
    bool isAction = false;
    float actionCount = 0.0f;       // 行動不能のカウント

    // 衝突
    float impactSpeed = 100.0f;        // 衝突のふっとび速度
    float impactInterval = 0.2f;    // 衝突の行動不能時間

    // ダッシュ
    bool isDash = false;
    float dashSpeed = 350.0f;       // ダッシュの速度
    float dashInterval = 0.7f;      // ダッシュの行動不能時間

    void Awake()
    {
        // 初期設定
        pRigidbody = this.GetComponent<Rigidbody>();
    }
    void Start()
    {
        // アクション時の操作不能時間を計測
        this.UpdateAsObservable()
            .Where(x => isAction)
            .Where(x => !GameManagement.Instance.isPause.Value)
            .Subscribe(_ =>
            {
                actionCount += -Time.deltaTime;
                if (actionCount <= 0.0f)
                {
                    isAction = false;
                }

            }).AddTo(this.gameObject);

        // 移動・方向転換
        pManager.MoveTrigger
            .Where(x => !pManager.isDeath.Value)
            .Where(x => !GameManagement.Instance.isPause.Value)
            .Subscribe(value =>
            {
                // 方向転換
                Vector3 moveRot = value.rightStickValue;
                if(ShopManager.Instance.)
                this.transform.localEulerAngles += moveRot * rotSpeed * Time.deltaTime;

                // 移動
                float speedMul = pManager.pData.pSpeedMul;
                float speedDown = 30.0f;
                Vector3 moveVec = Vector3.zero;
                moveVec += value.leftStickValue.x * transform.right;    // 左右方向
                moveVec += value.leftStickValue.y * transform.forward;  // 前後方向

                // 行動不能か否か
                if (isAction)
                {
                    // 静止力を弱める
                    speedMul *= 0.3f;
                }
                else
                {
                    pRigidbody.AddForce(moveVec.normalized * pManager.pData.pSpeed * speedMul);
                }

                // ダッシュの実行
                if (isDash)
                {
                    isDash = false;

                    // 入力方向にダッシュ、入力が0の場合は前方にダッシュ
                    if (value.leftStickValue != Vector3.zero)
                    {
                        moveVec *= 100.0f;
                    }
                    else
                    {
                        moveVec = transform.forward;
                    }
                    pRigidbody.AddForce(moveVec.normalized * dashSpeed, ForceMode.Impulse);
                }

                // 摩擦っぽい力
                pRigidbody.AddForce(-pRigidbody.velocity * speedMul * speedDown * Time.deltaTime);

            }).AddTo(this.gameObject);

        // ダッシュ
        pManager.DashTrigger
            .Subscribe(_ =>
            {
                // ダッシュの実行
                isDash = true;
                // 一定時間行動不能にする
                actionCount = dashInterval;
                isAction = true;

            }).AddTo(this.gameObject);

        // 衝突
        pManager.ImpactTrigger
            .Subscribe(value =>
            {
                Vector3 dir = (this.transform.position - value).normalized;
                pRigidbody.AddForce(dir * impactSpeed, ForceMode.Impulse);
                // 一定時間行動不能にする
                actionCount = impactInterval;
                isAction = true;

            }).AddTo(this.gameObject);


        pManager.isDeath
            .Where(x => x)
            .Subscribe(_ =>
            {
                pRigidbody.isKinematic = true;

            }).AddTo(this.gameObject);
        // リスポーン
        pManager.RespawnTrigger
            .Subscribe(_ =>
            {
                pRigidbody.isKinematic = false;
                this.transform.position = pRespawnPoint.position;

            }).AddTo(this.gameObject);
    }
}
