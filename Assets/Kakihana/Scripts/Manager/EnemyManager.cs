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
    // 敵のID
    [SerializeField] private int enemyID;
    // AI名称が格納されているクラス
    [SerializeField] AIListManager aiList;
    // 敵の移動量などを計算するクラス
    [SerializeField] AI_ActManager actManager;
    // ダメージカウント
    [SerializeField] private IntReactiveProperty hitCount = new IntReactiveProperty(0);   

    // 消滅時のエフェクト
    [SerializeField] ParticleSystem destroyPS;
    // バリアがあるか
    [SerializeField] BoolReactiveProperty isBarrier = new BoolReactiveProperty(false);
    // バリア
    [SerializeField] IntReactiveProperty enemyBarrier = new IntReactiveProperty(0);
    [SerializeField] private Rigidbody enemyRigid;          // 敵のRigidBody
    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    [SerializeField] private Vector3 movePos;               // 移動ベクトル
    [SerializeField] private Vector3 dif;
    [SerializeField] private int refrectPower;
    Subject<int> apprSubject = new Subject<int>();
    Subject<int> waitSubject = new Subject<int>();
    Subject<int> atkSubject = new Subject<int>();
    Subject<int> escSubject = new Subject<int>();

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
        // バリアの設定
        enemyBarrier.Value = enemyStatus.barrier;
    }

    // Start is called before the first frame update
    void Start()
    {
        atkSubject.Subscribe(val =>
        {
            Vector3 rad = (playerTrans.position - this.transform.position).normalized;
            float angle = Mathf.Atan2(rad.z, rad.x);
            actManager.EnemyAtkCalc(this.transform, val, angle);
        }).AddTo(this.gameObject);

        // エネミー消滅処理
        hitCount.Where(_ => hitCount.Value >= enemyStatus.hp).Subscribe(_ =>
        {
            // HPが0になったらステージクラスに消滅情報を送る
            StageManager.Instance.EnemyDestroy(this);
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
                    bullet.bulletState.Value = BulletManager.BulletState.Destroy;
                }
            }).AddTo(this.gameObject);

        this.OnCollisionEnterAsObservable()
    .Where(c => c.gameObject.tag != this.gameObject.tag)
    .Subscribe(c =>
    {
        if (c.gameObject.tag == "Block")
        {

        }
        else
        {
            isBarrier.Where(_ => isBarrier.Value == true)
            .Subscribe(_ =>
            {

            }).AddTo(this.gameObject);

            isBarrier.Where(_ => isBarrier.Value == false)
            .Subscribe(_ =>
            {
                dif = (c.transform.position - this.transform.position).normalized * refrectPower * Time.deltaTime;
            }).AddTo(this.gameObject);
        }
    }).AddTo(this.gameObject);

        GameManagement.Instance.playerUlt.Where(_ => GameManagement.Instance.playerUlt.Value == true)
        .Subscribe(_ =>
        {
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
            hitCount.Value++;
        }
    }
}
