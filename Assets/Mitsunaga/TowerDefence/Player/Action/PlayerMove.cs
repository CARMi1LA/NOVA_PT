using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMove : MonoBehaviour
{
    // 右スティックと左スティックの値を受け取り、Rigidbodyに力を加える(Addforce)
    Rigidbody pRigidbody;

    float rotSpeed = 360.0f;        // 回転速度

    // 何らかのアクション
    bool isAction = false;
    float actionCount = 0.0f;       // 行動不能のカウント

    // 衝突
    float impSpeed = 100.0f;        // 衝突のふっとび速度
    float impactInterval = 0.2f;    // 衝突の行動不能時間

    // ダッシュ
    bool isDash = false;
    float dashSpeed = 300.0f;       // ダッシュの速度
    float dashInterval = 0.3f;      // ダッシュの行動不能時間

    void Awake()
    {
        // 初期設定
        pRigidbody = this.GetComponent<Rigidbody>();

    }
    void Start()
    {
        // 衝突時の操作不能時間を計測
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
    }

    // 移動の実行
    public void ActionMove(InputValueData1P inputData,TDPlayerData pData)
    {
        // 方向転換
        Vector3 moveRot = inputData.rightStickValue;
        this.transform.localEulerAngles += moveRot * rotSpeed * Time.deltaTime;

        // 移動
        float speedMul = pData.pSpeedMul;
        Vector3 moveVec = Vector3.zero;
        moveVec += inputData.leftStickValue.x * transform.right;    // 左右方向
        moveVec += inputData.leftStickValue.y * transform.forward;  // 前後方向

        // 行動不能か否か
        if (isAction)
        {
            // 静止力を弱める
            speedMul *= 0.75f;
        }
        else
        {
            pRigidbody.AddForce(moveVec.normalized * pData.pSpeed * speedMul);
        }

        // ダッシュの実行
        if (isDash)
        {
            isDash = false;

            // 入力方向にダッシュ、入力が0の場合は前方にダッシュ
            if(inputData.leftStickValue != Vector3.zero)
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
        pRigidbody.AddForce(-pRigidbody.velocity * speedMul);
    }
    // 衝突
    public void ActionImpact(Vector3 targetPos)
    {
        Vector3 dir = (this.transform.position - targetPos).normalized;
        pRigidbody.AddForce(dir * impSpeed, ForceMode.Impulse);
        // 一定時間行動不能にする
        actionCount = impactInterval;
        isAction = true;
    }
    // ダッシュ
    public void ActionDash()
    {
        isDash = true;
        // 一定時間行動不能にする
        actionCount = dashInterval;
        isAction = true;
    }
}
