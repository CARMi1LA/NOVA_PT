using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDPlayerManager : MonoBehaviour
{
    /* 
     タワーディフェンス用のプレイヤーの行動管理
     * コントローラのインプットの割り当て
     * 各パラメータの変更
        などの処理

     * 攻撃
     * 移動
     * ダメージ
     * 死亡
     * 各アクション時エフェクト
        などの処理とマスターデータをつなぐ
     */
    
    // 変数の宣言
    
    // プレイヤーのデータ
    public TDPlayerData pData;
    InputValueData1P inputData;

    // デバッグ用の入力値の保存
    public Vector3 leftAxis, rightAxis;

    // ボタンイベント
    // 移動 <入力データ>
    public Subject<InputValueData1P>                moveTrigger     = new Subject<InputValueData1P>();
    // ダッシュ発動 <>
    public Subject<Unit>                            dashTrigger     = new Subject<Unit>();
    // 通常攻撃 <On/Off>
    public Subject<bool>                            attackTrigger   = new Subject<bool>();
    // スキル発動 <スキルの型>
    public Subject<TDPlayerData.SkillTypeList>      skillTrigger    = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動 <アルティメットの型>
    public Subject<TDPlayerData.UltimateTypeList>   ultimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();

    // アクションイベント
    // 衝突 <衝突ObjのPosition>
    public Subject<Vector3>                         impactTrigger   = new Subject<Vector3>();
    // ダメージ演出発動 <>
    public Subject<Unit>                            DamageTrigger   = new Subject<Unit>();
    // 死亡演出発動 <>
    public Subject<Unit>                            DeathTrigger    = new Subject<Unit>();

    void Awake()
    {
        // 初期設定
        // マスターから入力データとプレイヤーデータを取得
        //inputData = GameManagement.Instance.valueData1P;
        //pData     = GameManagement.Instance.playerData;

        // デバッグ用
        inputData = GetComponent<InputValueData1P>();
        pData = new TDPlayerData();
    }
    void Start()
    {
        // 更新処理
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // デバッグ用コントローラ入力取得
                {
                    leftAxis.x = Input.GetAxis("Horizontal");
                    leftAxis.y = Input.GetAxis("Vertical");
                    rightAxis.y = Input.GetAxis("RightStickHorizontal");
                    inputData.leftStickValue.x = leftAxis.x;
                    inputData.leftStickValue.y = leftAxis.y;
                    inputData.rightStickValue.y = rightAxis.x;
                    inputData.rightStickValue.y = Input.GetAxis("Mouse X");
                    inputData.pushBtnA.Value = Input.GetButton("Button_A");
                    inputData.pushBtnB.Value = Input.GetButton("Button_B");
                    inputData.pushBtnX.Value = Input.GetButton("Button_X");
                    inputData.pushBtnY.Value = Input.GetButton("Button_Y");
                    inputData.pushBtnRB.Value = Input.GetButton("Button_RB");
                    inputData.pushBtnLB.Value = Input.GetButton("Button_LB");
                }

                // スティック処理
                moveTrigger.OnNext(inputData);

            }).AddTo(this.gameObject);

        // 更新処理
        // エネルギー、アルティメットゲージの自動回復(雑)
        this.UpdateAsObservable()
            .SampleFrame(20)
            .Subscribe(_ =>
            {
                if(pData.pEnergy.Value < pData.pMaxEnergy)
                {
                    pData.pEnergy.Value++;
                }
                if(pData.pUltimate.Value < pData.pMaxUltimate)
                {
                    pData.pUltimate.Value++;
                }

            }).AddTo(this.gameObject);

        // ボタン処理
        // RBボタン：通常攻撃
        inputData.pushBtnRB.Subscribe(value =>
            {
                Debug.Log("通常攻撃：" + value.ToString());
                attackTrigger.OnNext(value);

            }).AddTo(this.gameObject);

        // LBボタン：未使用
        inputData.pushBtnLB
            .Where(x => x)
            .Subscribe(value =>
            {
                Debug.Log("LBキーは未使用です");

            }).AddTo(this.gameObject);

        // Aボタン：ダッシュ発動
        inputData.pushBtnA 
            .Where(x => x)                        // ボタンが押された
            .Where(x => pData.pEnergy.Value >= pData.pDashCost)          // エネルギーがある
            .ThrottleFirst(System.TimeSpan.FromSeconds(pData.pDashTime)) // ダッシュ中ではない
            .Subscribe(value =>
            {
                // ダッシュの実行
                Debug.Log("ダッシュ実行");
                dashTrigger.OnNext(Unit.Default);

                // エネルギーを消費する
                pData.pEnergy.Value -= pData.pDashCost;

            }).AddTo(this.gameObject);

        // Bボタン：各種アクセス
        inputData.pushBtnB
            .Where(x => x)
            .Subscribe(value =>
            {
                Debug.Log("アクセス");

            }).AddTo(this.gameObject);

        // Xボタン：スキル発動
        inputData.pushBtnX
            .ThrottleFirstFrame(pData.pSkillInterval)
            .Where(x => x)
            .Where(x => pData.pEnergy.Value >= pData.pSkillCost)
            .Subscribe(value =>
            {
                // スキルの実行
                Debug.Log("スキル実行：" + value.ToString());
                skillTrigger.OnNext(pData.pSkillType);

                // エネルギーを消費
                pData.pEnergy.Value -= pData.pSkillCost;

            }).AddTo(this.gameObject);

        // Yボタン：アルティメット発動
        inputData.pushBtnY
            .Where(x => x)
            .Where(x => pData.pUltimate.Value >= pData.pMaxUltimate)
            .Subscribe(value =>
            {
                // アルティメットの実行
                Debug.Log("アルティメット実行：" + value.ToString());
                ultimateTrigger.OnNext(pData.pUltimateType);
                // アルティメットゲージを消費
                pData.pUltimate.Value = 0;

            }).AddTo(this.gameObject);
    }
}
