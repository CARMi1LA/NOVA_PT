using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using Random = UnityEngine.Random;
public class EnemyManager : MonoBehaviour,IDamage
{
    // 敵のAI
    public enum EnemyAI
    {
        Approach = 0,       // 接近モード
        Wait,               // 待機モード
        Attack,             // 攻撃モード
        Escape              // 逃走モード
    }

    // 敵データのリスト
    [SerializeField] EnemyDataList enemyDataList;
    // 敵データが格納されているクラス
    [SerializeField] public EnemyStatus enemyStatus;
    // AI名称が格納されているクラス
    [SerializeField] AIListManager aiList;
    // 敵の移動量などを計算するクラス
    [SerializeField] AI_ActManager actManager;

    [SerializeField] private int enemyID;                   // 敵のID
    [SerializeField] IntReactiveProperty enemyHP;           // 現在のHP
    [SerializeField] private float maxHP;                   // 最大HP

    [SerializeField] ParticleSystem destroyPS;

    // 現在稼働しているAI
    [SerializeField] EnemyAIReactiveProperty enemyAI = new EnemyAIReactiveProperty();
    // プレイヤー間の距離
    [SerializeField] FloatReactiveProperty distance = new FloatReactiveProperty(0.0f);
    // 攻撃モードまでの待機時間
    [SerializeField] FloatReactiveProperty waitCount = new FloatReactiveProperty(0.0f);
    // 攻撃可能かどうかを管理するBool型プロパティ
    [SerializeField] BoolReactiveProperty attackFlg = new BoolReactiveProperty(false);

    [SerializeField] private Rigidbody enemyRigid;          // 敵のRigidBody
    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    [SerializeField] private Vector3 movePos;               // 移動ベクトル
    [SerializeField] private float maxDistance;             // プレイヤーとの最大接近距離
    [SerializeField] private float waitTimeLimit = 1.0f;    // 待機モードの最大遅延時間
    [SerializeField] private float atkTimeLimit = 1.0f;     // 攻撃モード時の攻撃する最大時間
    [SerializeField] private float velocityMag = 0.99f;     // 減速倍率

    Subject<int> apprSubject = new Subject<int>();
    Subject<int> waitSubject = new Subject<int>();
    Subject<int> atkSubject = new Subject<int>();
    Subject<int> escSubject = new Subject<int>();

    // 参照用のカスタムプロパティ
    [SerializeField]
    public IReadOnlyReactiveProperty<EnemyAI> enemyAIPropaty
    {
        get { return enemyAI; }
    }

    void Awake()
    {
        // IDより敵のデータリストを取得
        enemyDataList = Resources.Load<EnemyDataList>(string.Format("Enemy{0}", enemyID));
        // 現在のレベルより各パラメータを設定
        enemyStatus = enemyDataList.EnemyStatusList[GameManagement.Instance.gameLevel.Value - 1];
        // プレイヤーの座標を取得
        playerTrans = GameManagement.Instance.playerTrans;
        // 各タイプ別のAIリストを取得
        aiList = GameManagement.Instance.listManager;
        // AI処理クラスのコンポーネント取得
        actManager = GameManagement.Instance.actManager;
        enemyRigid = this.gameObject.GetComponent<Rigidbody>();
        // 最大HPの設定
        maxHP = enemyStatus.hp;
        // 現在のHPの設定
        enemyHP.Value = enemyStatus.hp;
        // 現在実行しているAIを設定
        enemyAI.Value = EnemyAI.Approach;
        // 各敵タイプ別に攻撃間隔と攻撃を行う距離の設定
        switch (enemyStatus.enemyType)
        {
            // 通常の敵の場合
            case EnemyStatus.EnemyType.Common:
                waitTimeLimit = 3.0f;
                maxDistance = 30.0f;
                break;
            // 中ボスの場合
            case EnemyStatus.EnemyType.Leader:
                waitTimeLimit = 3.0f;
                maxDistance = 50.0f;
                break;
            // ボスの場合
            case EnemyStatus.EnemyType.Boss:
                waitTimeLimit = 2.0f;
                maxDistance = 50.0f;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        apprSubject.Subscribe(val =>
        {
            movePos = actManager.CalcApprMove(this.transform.position, enemyStatus.moveSpeed, val);
        }).AddTo(this.gameObject);

        waitSubject.Subscribe(val => 
        {
            attackFlg.Value = true;
        }).AddTo(this.gameObject);

        atkSubject.Subscribe(val =>
        {
            Vector3 rad = (playerTrans.position - this.transform.position).normalized;
            float angle = Mathf.Atan2(rad.z, rad.x);
            actManager.EnemyAtkCalc(this.transform, val, angle);
            attackFlg.Value = false;
        }).AddTo(this.gameObject);

        escSubject.Subscribe(val =>
        {
            Vector3 diffPos = new Vector3(-1.0f, -1.0f, -1.0f);
            // 抽選結果より、逃走処理を行う
            movePos = actManager.CalcEscMove(this.transform.position, enemyStatus.moveSpeed * 0.5f, val);
            movePos = -movePos;
        }).AddTo(this.gameObject);


        // 敵のタイプ別に処理を行う
        switch (enemyStatus.enemyPosition)
        {
            // 攻撃タイプの処理
            case EnemyStatus.EnemyPosition.Attack:
                
                // 攻撃タイプのAIパターンを取得
                AI_NameListAttack AI_Atk = aiList.AI_AtkList;
                // AIレベルより行動確率パターンの取得
                AI_Atk.EnemyAIProbSet(enemyStatus.aiLevel);

                // 瀕死状態（最大HPの４分の１以下）になると逃走モードへ
                enemyHP.Where(_ => _ <= maxHP * 0.25f)
                    .Subscribe(_ =>
                    {
                        enemyAI.Value = EnemyAI.Escape;
                    }).AddTo(this.gameObject);

                // 攻撃タイプの接近モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Approach)
                    .Subscribe(_ =>
                    {
                        // 行動パターンを抽選
                        int actID = actManager.ChooseAppr(AI_Atk);
                        apprSubject.OnNext(actID);
                    }).AddTo(this.gameObject);

                // 攻撃タイプの待機モード処理
                enemyAIPropaty
                     .Where(_ => enemyAIPropaty.Value == EnemyAI.Wait)
                     .Where(_ => attackFlg.Value == false)
                     .Sample(TimeSpan.FromSeconds(2.0f))
                     .Subscribe(_ =>
                     {
                         int actID = actManager.ChooseWait(AI_Atk);
                         waitSubject.OnNext(actID);
                     }).AddTo(this.gameObject);

                // 攻撃タイプの攻撃モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Attack)
                    .Where(_ => attackFlg.Value == true)
                    .Sample(TimeSpan.FromSeconds(0.1f))
                    .Subscribe(_ =>
                    {
                        int actID = actManager.ChooseAtk(AI_Atk);
                        atkSubject.OnNext(actID);
                        enemyAI.Value = EnemyAI.Wait;
                    }).AddTo(this.gameObject);

                // 攻撃タイプの逃走モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Escape)
                    .Subscribe(_ =>
                    {
                        // 逃走パターンの抽選
                        int actID = actManager.ChooseEsc(AI_Atk);
                        escSubject.OnNext(actID);
                    }).AddTo(this.gameObject);
                break;
            // 防御タイプの処理
            case EnemyStatus.EnemyPosition.Defence:
                // 防御タイプのAIパターンを取得
                AI_NameListDefence AI_Def = aiList.AI_DefList;
                // AIレベルより行動確率パターンの取得
                AI_Def.EnemyAIProbSet(enemyStatus.aiLevel);

                // 瀕死状態（最大HPの４分の１以下）になると逃走モードへ
                enemyHP.Where(_ => _ <= maxHP * 0.25f)
                    .Subscribe(_ =>
                    {
                        enemyAI.Value = EnemyAI.Escape;
                    }).AddTo(this.gameObject);

                // 防御タイプの接近モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Approach)
                    .Subscribe(_ =>
                    {
                                // 行動パターンを抽選
                                int actID = actManager.ChooseAppr(AI_Def);
                                // 行動パターンに応じた移動量を取得
                                movePos = actManager.CalcApprMove(this.transform.position, enemyStatus.moveSpeed, actID);
                    }).AddTo(this.gameObject);

                // 防御タイプの待機モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Wait)
                    .Subscribe(_ =>
                    {
                        // 待機時間のカウント変数
                        float waitTime = 0.0f;
                        // 最大待機時間の倍率
                        float waitMag = 0.0f;                                // 待機モードの抽選を行う
                        int actID = actManager.ChooseWait(AI_Def);
                        // 抽選結果より、最大待機時間の設定
                        waitMag = actManager.ActWaitCalc(actID);

                        // 待機時間をカウント
                        waitTime += Time.deltaTime;
                        // 最大待機時間を超えたら攻撃モードへ、倍率により時間は変動する
                        if (waitTime >= waitTimeLimit * waitMag)
                        {
                            attackFlg.Value = true;
                        }
                    }).AddTo(this.gameObject);

                // 防御タイプの攻撃モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Attack)
                    .Sample(TimeSpan.FromSeconds(0.1f))
                    .Subscribe(_ =>
                    {
                        // 攻撃時間のカウント変数
                        float atkTime = 0.0f;
                        // 攻撃時間を計算
                        atkTime += Time.deltaTime;
                        // 攻撃パターンの抽選
                        int actID = actManager.ChooseAtk(AI_Def);
                        // 抽選結果より、各種攻撃を行う
                        actManager.EnemyAtkCalc(this.transform, actID, atkTime);
                        // リミットまでの時間攻撃を行い、待機モードに移行
                        if (atkTime >= atkTimeLimit)
                        {
                            attackFlg.Value = false;
                            enemyAI.Value = EnemyAI.Wait;
                        }
                    }).AddTo(this.gameObject);

                // 防御タイプの逃走モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Escape)
                    .Subscribe(_ =>
                    {
                        // 逃走パターンの抽選
                        int actID = actManager.ChooseEsc(AI_Def);
                        // 抽選結果より、逃走処理を行う
                        movePos = actManager.CalcEscMove(this.transform.position, enemyStatus.moveSpeed, actID);

                        movePos = -movePos;
                    }).AddTo(this.gameObject);
                break;
            case EnemyStatus.EnemyPosition.Special:
                break;
            // 中ボスタイプの処理
            // 中ボスは通常攻撃の他に一定体力以下で強力な攻撃を行う
            case EnemyStatus.EnemyPosition.Leader:
                // 中ボスのAIパターンを取得
                AI_NameListLeader AI_Leader = aiList.AI_LeaderList;
                // AIレベルより行動確率パターンの取得
                AI_Leader.EnemyAIProbSet(enemyStatus.aiLevel);
                // ボスの必殺技パターン用変数
                int leaderAtkIndex = 0;
                // 必殺技使用回数
                int leaderAtkCount = 0;
                // 中ボスタイプの接近モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Approach)
                    .Subscribe(_ =>
                    {
                        // 行動パターンに応じた移動量を取得
                        movePos = actManager.CalcApprMove(this.transform.position, enemyStatus.moveSpeed, (int)AI_NameListLeader.AI_Approach.Normal);
                    }).AddTo(this.gameObject);

                // 中ボスタイプの待機モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Wait)
                    .Subscribe(_ =>
                    {
                        // 待機時間のカウント変数
                        float waitTime = 0.0f;
                                // 最大待機時間の倍率
                                float waitMag = 0.0f;
                                // 待機モードの抽選を行う
                                int actID = actManager.ChooseWait(AI_Leader);
                                // 抽選結果より、最大待機時間の設定
                                waitMag = actManager.ActWaitCalc(actID);

                                // 待機時間をカウント
                                waitTime += Time.deltaTime;
                                // 最大待機時間を超えたら攻撃モードへ、倍率により時間は変動する
                                if (waitTime >= waitTimeLimit * waitMag)
                        {
                            attackFlg.Value = true;
                        }
                    }).AddTo(this.gameObject);
                // 中ボスタイプの攻撃モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Attack)
                    .Sample(TimeSpan.FromSeconds(0.1f))
                    .Subscribe(_ =>
                    {
                        // 攻撃時間のカウント変数
                        float atkTime = 0.0f;
                        // 攻撃時間を計算
                        atkTime += Time.deltaTime;
                        // 攻撃パターンの抽選
                        int actID = actManager.ChooseAtk(AI_Leader);
                        // 通常攻撃はランダムで行うが一定体力以下で強力な攻撃を行う
                        if (enemyHP.Value <= maxHP * 0.75f && leaderAtkCount == 0)
                        {
                            // HPが75%以下
                            actManager.EnemyAtkCalc(this.transform, actManager.leaderAtkList[leaderAtkIndex], atkTime);
                            leaderAtkCount++;
                            leaderAtkIndex++;
                        }
                        else if (enemyHP.Value <= maxHP * 0.5f && leaderAtkCount == 1)
                        {
                                    // HPが50%以下
                                    actManager.EnemyAtkCalc(this.transform, actManager.leaderAtkList[leaderAtkIndex], atkTime);
                            leaderAtkCount++;
                            leaderAtkIndex++;
                        }
                        else if (enemyHP.Value <= maxHP * 0.25f && leaderAtkCount == 2)
                        {
                                    // HPが25%以下
                                    actManager.EnemyAtkCalc(this.transform, actManager.leaderAtkList[leaderAtkIndex], atkTime);
                            leaderAtkCount++;
                            leaderAtkIndex++;
                        }
                        else
                        {
                                    // リミットまでの時間攻撃を行い、待機モードに移行
                                    if (atkTime >= atkTimeLimit)
                            {
                                attackFlg.Value = false;
                                enemyAI.Value = EnemyAI.Wait;
                            }
                                    // 抽選結果より、各種攻撃を行う
                                    actManager.EnemyAtkCalc(this.transform, actID, atkTime);
                        }


                    }).AddTo(this.gameObject);
                break;
            // ボスタイプの処理
            // ボスクラスの攻撃はランダムではなく、順番に攻撃を行う
            case EnemyStatus.EnemyPosition.Boss:
                // ボスのAI情報取得
                AI_NameListBoss AI_Boss = aiList.AI_BossList;

                AI_Boss.EnemyAIProbSet(enemyStatus.aiLevel);
                // ボスの攻撃パターン用変数
                int bossAtkIndex = 0;
                // 設定されているパターン数を超えたらリセットする
                if (bossAtkIndex >= actManager.bossAtkList.Length)
                {
                    bossAtkIndex = 0;
                }
                // ボスタイプの接近モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Approach)
                    .Subscribe(_ =>
                    {
                                // 行動パターンに応じた移動量を取得
                                movePos = actManager.CalcApprMove(this.transform.position, enemyStatus.moveSpeed, (int)AI_NameListBoss.AI_Approach.Normal);
                    }).AddTo(this.gameObject);

                // ボスタイプの待機モード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Wait)
                    .Subscribe(_ =>
                    {
                                // 待機時間のカウント変数
                                float waitTime = 0.0f;
                                // 最大待機時間の倍率
                                float waitMag = 0.0f;
                                // 待機モードの抽選を行う
                                int actID = actManager.ChooseWait(AI_Boss);
                                // 抽選結果より、最大待機時間の設定
                                waitMag = actManager.ActWaitCalc(actID);

                                // 待機時間をカウント
                                waitTime += Time.deltaTime;
                                // 最大待機時間を超えたら攻撃モードへ、倍率により時間は変動する
                                if (waitTime >= waitTimeLimit * waitMag)
                        {
                            attackFlg.Value = true;
                        }
                    }).AddTo(this.gameObject);

                // ボスタイプのモード処理
                enemyAIPropaty.Where(_ => _ == EnemyAI.Attack)
                    .Sample(TimeSpan.FromSeconds(0.1f))
                    .Subscribe(_ =>
                    {
                                // 攻撃時間のカウント変数
                                float atkTime = 0.0f;
                                // 攻撃時間を計算
                                atkTime += Time.deltaTime;
                                // 必殺技使用回数
                                int bossAtkCount = 0;
                                // 攻撃パターンの設定
                                int actID = actManager.bossAtkList[bossAtkIndex];
                                // 攻撃時間をカウント
                                atkTime += Time.deltaTime;
                                // ボスクラスは通常攻撃の他に一定体力以下で強力な攻撃を行う
                                if (enemyHP.Value <= maxHP * 0.8f && bossAtkCount == 0)
                        {
                                    // HPが80%以下
                                    actManager.EnemyAtkCalc(this.transform, (int)AI_NameListBoss.AI_Attack.WhirlFireCombo, atkTime);
                            bossAtkCount++;
                        }
                        else if (enemyHP.Value <= maxHP * 0.5f && bossAtkCount == 1)
                        {
                                    // HPが50%以下
                                    actManager.EnemyAtkCalc(this.transform, (int)AI_NameListBoss.AI_Attack.BoostBoundRayCombo, atkTime);
                            bossAtkCount++;
                        }
                        else if (enemyHP.Value <= maxHP * 0.3f && bossAtkCount == 2)
                        {
                                    // HPが30%以下
                                    actManager.EnemyAtkCalc(this.transform, (int)AI_NameListBoss.AI_Attack.WhirlBoostCombo, atkTime);
                            bossAtkCount++;
                        }
                        else if (enemyHP.Value <= maxHP * 0.1f && bossAtkCount == 3)
                        {
                                    // HPが10%以下
                                    actManager.EnemyAtkCalc(this.transform, (int)AI_NameListBoss.AI_Attack.Ultimate, atkTime);
                            bossAtkCount++;
                        }
                        else
                        {
                                    // 通常攻撃
                                    actManager.EnemyAtkCalc(this.transform, actID, atkTime);
                        }
                        if (atkTime >= atkTimeLimit)
                        {
                            attackFlg.Value = false;
                            enemyAI.Value = EnemyAI.Wait;
                            bossAtkIndex++;
                        }
                    }).AddTo(this.gameObject);
                break;
            case EnemyStatus.EnemyPosition.Player:
                break;
            default:
                break;
        }

        this.UpdateAsObservable()
            .Where(p => GameManagement.Instance.isPause.Value == false)
            .Subscribe(p => 
            {
                /*
                 以下、全敵キャラクター共通処理
               */

                // 【接近モード移行イベント】
                // 敵が最大接近距離よりも遠ければ接近モードへ移行する
                distance.Where(_ => _ >= Mathf.Pow(maxDistance, 2))
                    .Subscribe(_ =>
                    {
                        // 減速移動量の設定
                        velocityMag = 0.99f;
                        // AIモードを接近モードに
                        enemyAI.Value = EnemyAI.Approach;
                    }).AddTo(this.gameObject);

                // 【待機モード移行イベント】
                // 敵が最大接近距離に到達したら減速し次の行動を待つ
                distance.Where(_ => _ <= Mathf.Pow(maxDistance, 2))
                    .Where(_ => enemyAIPropaty.Value == EnemyAI.Approach)
                    .Where(_ => attackFlg.Value == false)
                    .Subscribe(_ =>
                    {
                        // 減速移動量の設定
                        velocityMag = 0.66f;
                        // AIモードを待機モードに
                        enemyAI.Value = EnemyAI.Wait;
                    }).AddTo(this.gameObject);

                // 攻撃フラグがONになったら攻撃モードへ移行
                attackFlg.Where(_ => _ = attackFlg.Value == true && enemyAIPropaty.Value == EnemyAI.Wait)
                .Subscribe(_ =>
                {
                    enemyAI.Value = EnemyAI.Attack;
                }).AddTo(this.gameObject);

                // 移動時、徐々に減速し、プレイヤー間の距離の計算を行う
                this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ =>
                {
                    enemyRigid.velocity *= velocityMag;
                    distance.Value = actManager.CalcDistance(this.transform.position);
                }).AddTo(this.gameObject);

                // 各AIにより算出された移動量をもとに移動処理を行う
                enemyRigid.velocity += movePos * Time.deltaTime;
                if (enemyAI.Value == EnemyAI.Attack || enemyAI.Value == EnemyAI.Wait)
                {
                    movePos = Vector3.zero;
                }
            }).AddTo(this.gameObject);

        // エネミー消滅処理
        enemyHP.Where(_ => enemyHP.Value <= 0).Subscribe(_ =>
        {
            // HPが0になったらステージクラスに消滅情報を送る
            StageManager.Instance.EnemyDestroy(this);
        }).AddTo(this.gameObject);

        GameManagement.Instance.isPause
            .Where(p => true)
            .Subscribe(p =>
            {
               
            }).AddTo(this.gameObject);

        // 衝突判定
        this.OnTriggerEnterAsObservable()
            .Where(c => c.gameObject.tag == "Bullet")
            .Subscribe(c =>
            {
                // 弾のコンポーネント取得
                BulletManager bullet;
                bullet = c.gameObject.GetComponent<BulletManager>();
                if (bullet.shootChara == BulletManager.ShootChara.Player)
                {
                    // プレイヤーによる攻撃であればダメージを受ける
                    HitDamage();
                    // 衝突した弾は消滅する
                    bullet.BulletDestroy();
                }
            }).AddTo(this.gameObject);

        GameManagement.Instance.playerUlt.Where(_ => GameManagement.Instance.playerUlt.Value == true)
        .Subscribe(_ =>
        {
            Debug.Log("ult");
            Instantiate(destroyPS, this.transform.position, Quaternion.identity);
            StageManager.Instance.EnemyDestroy(this);
        }).AddTo(this.gameObject);
    }

    public void HitDamage()
    {
        GameManagement.Instance.DamageScore();
        GameManagement.Instance.HitCombo();
        // １ヒットごとに１ダメージ受ける
        // バリアが残っていればバリアを優先的に消費する
        if (enemyStatus.barrier >= 1)
        {
            enemyStatus.barrier--;
        }else if(enemyStatus.barrier <= 0)
        {
            enemyHP.Value--;
        }
    }
}
// 敵AI専用のカスタムプロパティ
[System.Serializable]
public class EnemyAIReactiveProperty : ReactiveProperty<EnemyManager.EnemyAI>
{
    public EnemyAIReactiveProperty() { }
    public EnemyAIReactiveProperty(EnemyManager.EnemyAI initialValue) : base(initialValue) { }
}
