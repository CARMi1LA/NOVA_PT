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

    TDPlayerData pData;

    Rigidbody pRigidbody;   // 物理
    [SerializeField] PlayerCollision    pCollision; // 当たり判定処理
    [SerializeField] PlayerMove         pMove;      // 移動処理     トランスフォームを扱う
    [SerializeField] PlayerAttack       pAttack;    // 攻撃処理     弾の生成を扱う

    // スキル発動イベント
    public Subject<TDPlayerData.SkillTypeList> skillTrigger = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動イベント
    public Subject<TDPlayerData.UltimateTypeList> ultimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();
    // ダメージ演出発動イベント
    public Subject<Unit> DamageTrigger = new Subject<Unit>();
    // 死亡演出発動イベント
    public Subject<Unit> DeathTrigger = new Subject<Unit>();


    void Awake()
    {
        // 初期設定
        pRigidbody = this.GetComponent<Rigidbody>();
    }
    void Start()
    {
        // コントローラの入力を取得
        InputValueData1P inputData = GameManagement.Instance.valueData1P;
        pData = new TDPlayerData();

        // ボタン処理
        inputData.pushBtnRB.Subscribe(value =>
            {
                Debug.Log("通常攻撃");

            }).AddTo(this.gameObject);
        inputData.pushBtnLB.Subscribe(value =>
            {
                Debug.Log("未使用");

            }).AddTo(this.gameObject);
        inputData.pushBtnA .Subscribe(value =>
            {
                Debug.Log("緊急回避");

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

        // 更新処理
        this.UpdateAsObservable()
            .Where(x => true/* ポーズ中でなければ */)
            .Subscribe(_ =>
            {
                // スティック処理
                pMove.ActionMove(inputData);
            }).AddTo(this.gameObject);
    }
}
