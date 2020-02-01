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

    public TDList.ParentList pParent = TDList.ParentList.Player;   // 陣営データ

    // デバッグ用の入力値の保存
    public Vector3 leftAxis, rightAxis;

    // ボタンイベント
    // 移動 <入力データ>
    public Subject<InputValueData1P>                MoveTrigger     = new Subject<InputValueData1P>();
    // ダッシュ発動 <>
    public Subject<Unit>                            DashTrigger     = new Subject<Unit>();
    // 通常攻撃 <On/Off>
    public Subject<bool>                            AttackTrigger   = new Subject<bool>();
    // スキル発動 <スキルの型>
    public Subject<TDPlayerData.SkillTypeList>      SkillTrigger    = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動 <アルティメットの型>
    public Subject<TDPlayerData.UltimateTypeList>   UltimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();

    // アクションイベント
    // 衝突 <衝突ObjのPosition>
    public Subject<Vector3>                         ImpactTrigger   = new Subject<Vector3>();
    // ダメージ演出発動 <>
    public Subject<Unit>                            DamageTrigger   = new Subject<Unit>();
    // 回復イベント
    public Subject<Unit>                            HealTrigger     = new Subject<Unit>();
    // リスポーンイベント
    public Subject<Unit>                            RespawnTrigger  = new Subject<Unit>();
    
    // 死亡判定
    public BoolReactiveProperty                     isDeath         = new BoolReactiveProperty();

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
        // 入力
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                // デバッグ用コントローラ入力取得
                {
                    leftAxis.x = Input.GetAxis("Horizontal");
                    leftAxis.y = Input.GetAxis("Vertical");
                    if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f)
                    {
                        rightAxis.y = Input.GetAxis("Mouse X");
                    }
                    else if (Mathf.Abs(Input.GetAxis("RightStickHorizontal")) > 0.1f)
                    {
                        rightAxis.y = Input.GetAxis("RightStickHorizontal");
                    }
                    else
                    {
                        rightAxis.y = 0.0f;
                    }

                    inputData.leftStickValue = leftAxis;
                    inputData.rightStickValue = rightAxis;
                    inputData.pushBtnA.Value = Input.GetButton("Button_A");
                    inputData.pushBtnB.Value = Input.GetButton("Button_B");
                    inputData.pushBtnX.Value = Input.GetButton("Button_X");
                    inputData.pushBtnY.Value = Input.GetButton("Button_Y");
                    inputData.pushBtnRB.Value = Input.GetButton("Button_RB");
                    inputData.pushBtnLB.Value = Input.GetButton("Button_LB");
                }

                // 移動処理
                MoveTrigger.OnNext(inputData);

            }).AddTo(this.gameObject);

        // プレイヤーのレベルアップ処理
        ShopManager.Instance.spLv.playerLv.lv_HP
            .Subscribe(value => 
            {
                pData.SetMaxHealth(value);

            }).AddTo(this.gameObject);
        ShopManager.Instance.spLv.playerLv.lv_Spd
            .Subscribe(value =>
            {
                pData.SetSpeed(value);

            }).AddTo(this.gameObject);
        ShopManager.Instance.spLv.playerLv.lv_Int
            .Subscribe(value =>
            {
                pData.SetShotInterval(value);

            }).AddTo(this.gameObject);
        // スキル、アルティメットの変更処理
        ShopManager.Instance.spLv.skillLv.level_Skill
            .Subscribe(value =>
            {
                TDPlayerData.SkillTypeList sType = TDPlayerData.SkillTypeList.Sword;

                switch (value)
                {
                    case 0:
                        sType = TDPlayerData.SkillTypeList.Sword;
                        break;
                    case 1:
                        sType = TDPlayerData.SkillTypeList.Razer;
                        break;
                    case 2:
                        sType = TDPlayerData.SkillTypeList.Missile;
                        break;
                    case 3:
                        sType = TDPlayerData.SkillTypeList.Bash;
                        break;
                    default:
                        break;
                }

                pData.pSkillType = sType;

            }).AddTo(this.gameObject);
        ShopManager.Instance.spLv.ultLv.level_Ult
            .Subscribe(value =>
            {


            }).AddTo(this.gameObject);

        // エネルギー、アルティメットゲージの自動回復(雑)
        this.UpdateAsObservable()
            .Where(x => !GameManagement.Instance.isPause.Value)
            .Sample(System.TimeSpan.FromSeconds(0.02f))
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

        // 死亡判定
        pData.pHealth
            .Where(x => x <= 0)
            .Subscribe(value =>
            {
                isDeath.Value = true;

            }).AddTo(this.gameObject);
        // リスポーン処理
        RespawnTrigger
            .Subscribe(_ =>
            {
                pData.pHealth.Value = pData.pMaxHealth;

                isDeath.Value = false;

            }).AddTo(this.gameObject);

        // ダメージ処理
        DamageTrigger
            .Subscribe(_ =>
            {
                pData.pHealth.Value -= 1;

            }).AddTo(this.gameObject);
        // 回復処理
        HealTrigger
            .Subscribe(_ =>
            {
                pData.pHealth.Value = pData.pMaxHealth;

            }).AddTo(this.gameObject);

        // ボタン処理
        {
            // RBボタン：通常攻撃
            inputData.pushBtnRB.Subscribe(value =>
                {
                    AttackTrigger.OnNext(value);

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
                .Where(x => x)                                                  // ボタンが押された
                .Where(x => !isDeath.Value)
                .Where(x => !GameManagement.Instance.isPause.Value)
                .Where(x => pData.pEnergy.Value >= pData.pDashCost)             // エネルギーがある
                .ThrottleFirst(System.TimeSpan.FromSeconds(pData.pDashTime))    // ダッシュ中ではない
                .Subscribe(value =>
                {
                    // ダッシュの実行
                    DashTrigger.OnNext(Unit.Default);
                    // エネルギーを消費する
                    pData.pEnergy.Value -= pData.pDashCost;

                }).AddTo(this.gameObject);

            // Bボタン：各種アクセス
            inputData.pushBtnB
                .Where(x => x)
                .Where(x => !isDeath.Value)
                .Where(x => !GameManagement.Instance.isPause.Value)
                .Subscribe(value =>
                {
                    Debug.Log("アクセス");

                }).AddTo(this.gameObject);

            // Xボタン：スキル発動
            inputData.pushBtnX
                .ThrottleFirst(System.TimeSpan.FromSeconds(pData.pSkillInterval))
                .Where(x => x)
                .Where(x => !isDeath.Value)
                .Where(x => !GameManagement.Instance.isPause.Value)
                .Where(x => pData.pEnergy.Value >= pData.pSkillCost)
                .Subscribe(value =>
                {
                    // スキルの実行
                    SkillTrigger.OnNext(pData.pSkillType);
                    // エネルギーを消費
                    pData.pEnergy.Value -= pData.pSkillCost;

                }).AddTo(this.gameObject);

            // Yボタン：アルティメット発動
            inputData.pushBtnY
                .Where(x => x)
                .Where(x => !isDeath.Value)
                .Where(x => !GameManagement.Instance.isPause.Value)
                .Where(x => pData.pUltimate.Value >= pData.pMaxUltimate)
                .Subscribe(value =>
                {
                    // アルティメットの実行
                    UltimateTrigger.OnNext(pData.pUltimateType);
                    // アルティメットゲージを消費
                    pData.pUltimate.Value = 0;

                }).AddTo(this.gameObject);
        }
    }
}
