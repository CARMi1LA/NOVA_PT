using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDPlayerManager : MonoBehaviour
{
    /* 
     タワーディフェンス用のプレイヤーの行動管理
     * 攻撃
     * 移動
     * ダメージ
     * 死亡
     などの処理とマスターデータをつなぐ
     */
    
    // 変数の宣言
    
    // プレイヤーのデータ
    public TDPlayerData pData;
    InputValueData1P inputData;
    // デバッグ用の入力値の保存
    public Vector3 leftAxis, rightAxis;

    [SerializeField] PlayerCollision    pCollision; // 当たり判定処理   Colliderの管理
    [SerializeField] PlayerMove         pMove;      // 移動処理         Rigidbody及びTransformの管理

    // イベントの宣言
    // 通常攻撃のOn/Offイベント
    public Subject<bool> attackTrigger = new Subject<bool>();
    // スキル発動イベント
    public Subject<TDPlayerData.SkillTypeList>      skillTrigger    = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動イベント
    public Subject<TDPlayerData.UltimateTypeList>   ultimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();
    // ダメージ演出発動イベント
    public Subject<Unit>                            DamageTrigger   = new Subject<Unit>();
    // 死亡演出発動イベント
    public Subject<Unit>                            DeathTrigger    = new Subject<Unit>();

    void Awake()
    {
        // 初期設定
        // デバッグ用
        inputData = GetComponent<InputValueData1P>();
    }
    void Start()
    {
        // コントローラの入力を取得
        //InputValueData1P inputData = GameManagement.Instance.valueData1P;
        pData = new TDPlayerData();

        // 更新処理
        (this).UpdateAsObservable()
            .Subscribe(_ =>
            {
                // デバッグ用コントローラ入力取得
                leftAxis.x = Input.GetAxis("Horizontal");
                leftAxis.y = Input.GetAxis("Vertical");
                rightAxis.y = Input.GetAxis("RightStickHorizontal");
                inputData.leftStickValue.x = leftAxis.x;
                inputData.leftStickValue.y = leftAxis.y;
                inputData.rightStickValue.y = rightAxis.x;
                inputData.rightStickValue.y = Input.GetAxis("Mouse X");
                if (Input.GetButton("Button_A"))
                {
                    inputData.pushBtnA.Value = true;
                }
                else
                {
                    inputData.pushBtnA.Value = false;
                }

                // スティック処理
                pMove.ActionMove(inputData, pData);

            }).AddTo(this.gameObject);

        // 衝突イベント処理
        pCollision.cImpactSubject
            .Subscribe(value =>
            {
                pMove.ActionImpact(value);

            }).AddTo(this.gameObject);

        // ボタン処理
        inputData.pushBtnRB.Subscribe(value =>
            {
                Debug.Log("通常攻撃");

            }).AddTo(this.gameObject);
        inputData.pushBtnLB.Subscribe(value =>
            {
                Debug.Log("未使用");

            }).AddTo(this.gameObject);
        inputData.pushBtnA 
            .Where(x => inputData.pushBtnA.Value)
            .Subscribe(value =>
            {
                Debug.Log("緊急回避");
                pMove.ActionDash();

            }).AddTo(this.gameObject);
        inputData.pushBtnB .Subscribe(value =>
            {
                Debug.Log("アクセス");

            }).AddTo(this.gameObject);
        // Xボタン：スキル発動
        inputData.pushBtnX 
            .Where(x => pData.pEnergy.Value >= pData.pSkillCost)
            .Subscribe(value =>
            {
                Debug.Log("スキル発動");
                skillTrigger.OnNext(pData.pSkillType);

            }).AddTo(this.gameObject);
        // Yボタン：アルティメット発動
        inputData.pushBtnY 
            .Where(x => pData.pUltimate.Value >= pData.pMaxUltimate)
            .Subscribe(value =>
            {
                Debug.Log("アルティメット発動");
                ultimateTrigger.OnNext(pData.pUltimateType);

            }).AddTo(this.gameObject);
    }
}
