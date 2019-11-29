using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDPlayerManager : MonoBehaviour,IDamage,ICollision
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

    [SerializeField] PlayerDamage   pDamage;    // ダメージ処理
    [SerializeField] PlayerDeath    pDeath;     // 死亡処理　
    [SerializeField] PlayerMove     pMove;      // 移動処理　トランスフォームを扱う
    [SerializeField] PlayerAttack   pAttack;    // 攻撃処理　弾の生成を扱う

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
        inputData.pushBtnX .Subscribe(value =>
            {
                Debug.Log("スキル発動");

            }).AddTo(this.gameObject);
        inputData.pushBtnY .Subscribe(value =>
            {
                Debug.Log("アルティメット発動");

            }).AddTo(this.gameObject);

        // 更新処理
        this.UpdateAsObservable()
            .Where(x => true/* ポーズ中でなければ */)
            .Subscribe(_ =>
            {
                // スティック処理
                pMove.ActionMove(inputData);
            }).AddTo(this.gameObject);

        // 衝突判定
        this.OnCollisionEnterAsObservable()
            .Subscribe(col =>
            {
                // 相手をふっとばす
                if (col.gameObject.GetComponent<ICollision>() != null)
                {
                    col.gameObject.GetComponent<ICollision>().HitCollision(this.transform.position);
                }
                // エネミーのオブジェクトと衝突した場合
                if(col.gameObject.tag == "Enemy")
                {
                    HitDamage();
                }
            }).AddTo(this.gameObject);

        // 重複判定
        this.OnTriggerEnterAsObservable()
            .Subscribe(col =>
            {
                // エネミーのオブジェクトと重複した場合
                if(col.gameObject.tag == "Enemy")
                {
                    HitDamage();
                }
            }).AddTo(this.gameObject);
    }

    public void HitDamage()
    {
        // ダメージ処理
        pDamage.ActionDamage();

        if (true /* 現在HP <= 0 */)
        {
            // 死亡処理
            pDeath.ActionDeath();
        }
    }
    public void HitCollision(Vector3 targetPos)
    {
        Debug.Log("相手も反発できる");
    }
}
