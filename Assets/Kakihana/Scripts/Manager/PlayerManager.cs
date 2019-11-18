using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public class PlayerManager : BulletSetting,IDamage
{

    // プレイヤースクリプト

    // 速度モード
    public enum Speed
    {
        High = 0,   // 高速
        Law = 1     // 低速
    }

    [SerializeField] EnemyDataList dataList;                      // パラメータデータリスト
    [SerializeField] EnemyStatus status;                          // プレイヤーのパラメータ
    [SerializeField] HUD_Model playerHUD;                         // UI
    [SerializeField] SwordManager swordManager;

    [SerializeField] private IntReactiveProperty playerHp;        // 現在のHP
    [SerializeField] private IntReactiveProperty maxHp;           // 最大HP
    [SerializeField] private IntReactiveProperty energy;          // エネルギー
    [SerializeField] private IntReactiveProperty ultimateGage;    // 必殺技ゲージ
    [SerializeField] private IntReactiveProperty playerBarrier;
    [SerializeField] private Rigidbody playerRigid;               // プレイヤーのRigidbody
    [SerializeField] Speed speed = Speed.Law;                     // 速度モード

    [SerializeField] private int maxBarrier;                      // 最大バリア数
    [SerializeField] private int maxEnergy;
    [SerializeField] private int skillCost = 30;
    [SerializeField] private int maxultimateGage = 100;
    [SerializeField] private float playerSpeed = 10.0f;           // プレイヤーの速度

    // レベルを監視可能な変数
    [SerializeField] public IntReactiveProperty level;
    // クリックしたか
    [SerializeField] public BoolReactiveProperty isClick = new BoolReactiveProperty(false);
    // マウスの座標
    [SerializeField] private Vector3 cScreen;
    // マウスのワールド座標
    [SerializeField] public Vector3 cWorld;
    // 進行方向の単位ベクトル
    [SerializeField] private Vector3 dif;
    // マウス角度
    [SerializeField] private Vector3 rot;
    // 回転角度
    [SerializeField] public float angle;
    // 反発力
    [SerializeField] public float refrectPower;
    // 子機のトランスフォーム
    [SerializeField] private Transform bitRight,bitLeft;

    // スコアを監視可能な変数
    public IntReactiveProperty score = new IntReactiveProperty(0);
    // 被弾直後かどうか
    [SerializeField] private BoolReactiveProperty isHit = new BoolReactiveProperty(false);
    // バリアがあるかどうか
    private BoolReactiveProperty isBarrier = new BoolReactiveProperty(false);
    // 無敵状態かどうか
    private BoolReactiveProperty invincible = new BoolReactiveProperty(false);

    [SerializeField] private ParticleSystem  deathPS,ultPS;
    private Renderer playerRenderer;

    Subject<int> ultimate = new Subject<int>();
    Subject<int> skillSubject = new Subject<int>();
    Subject<BulletList> shootSubject = new Subject<BulletList>();
    Subject<Transform> shootTest = new Subject<Transform>();
    Subject<float> FlashSubject = new Subject<float>();


    void Awake()
    {
        // UI上の最大HP設定
        playerHUD.maxHealth = maxHp.Value;
        // UI上の最大エネルギーの設定
        playerHUD.maxEnergy = maxEnergy;
        // UI上の最大バリア設定
        playerHUD.maxBarrier = maxBarrier;
        // UI上の最大必殺技ゲージの設定
        playerHUD.maxUltimate = maxultimateGage;

        // UIにHPを設定
        playerHUD.HealthRP.Value = playerHp.Value;
        // UIにバリアを設定
        playerHUD.BarrierRP.Value = playerBarrier.Value;
        // UIにエネルギーを設定
        playerHUD.EnergyRP.Value = energy.Value;
        // UIにULTゲージを表示
        playerHUD.UltimateRP.Value = ultimateGage.Value;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = this.GetComponent<Renderer>();
        // 弾発射処理
        shootSubject.Subscribe(val =>
        {
            switch (val)
            {
                // 通常弾
                case BulletList.Normal:
                    GameManagement.Instance.bulletActManager.BulletShootSet(bitLeft.transform,BulletList.Normal, BulletManager.ShootChara.Player, 0.2f);
                    GameManagement.Instance.bulletActManager.BulletShootSet(bitRight.transform, BulletList.Normal, BulletManager.ShootChara.Player, 0.2f);
                    break;
            }
        }).AddTo(this.gameObject);

        skillSubject.Subscribe(skill => 
        {
            energy.Value -= skillCost;
            playerHUD.EnergyRP.Value = energy.Value;
            swordManager.SwordSubject.OnNext(0.7f);
        }).AddTo(this.gameObject);

        // 必殺技処理
        ultimate.Subscribe(val => 
        {
            Instantiate(ultPS, this.transform.position, Quaternion.identity);
            GameManagement.Instance.playerUlt.Value = true;
            ultimateGage.Value -= maxultimateGage;
            playerHUD.UltimateRP.Value = ultimateGage.Value;
        }).AddTo(this.gameObject);

        FlashSubject
            .Do(_ => playerRenderer.material.SetInt("_IsDamage", 1))
            .Delay(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => 
            {
                playerRenderer.material.SetInt("_IsDamage", 0);
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Subscribe(_ =>
            {
                // マウスのスクリーン座標を取得
                cScreen = Input.mousePosition;
                // カメラの焦点を補正
                cScreen.z = 50.0f;
                // スクリーン座標からワールド座標へ変換
                cWorld = Camera.main.ScreenToWorldPoint(cScreen);
                // マウスホイールの回転量取得
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                
                // 単位ベクトルの方向に移動する、Y軸は常に0に
                Vector3 movePos = this.transform.position + dif;
                rot = (cWorld - this.transform.position).normalized;
                angle = Mathf.Rad2Deg * Mathf.Atan2(rot.x, rot.z);
                movePos.y = 0.0f;

                switch (speed)
                {
                    case Speed.High:
                        // マウスのワールド座標より、進行方向の単位ベクトルを取得する
                        dif = (cWorld - this.transform.position).normalized * playerSpeed * Time.deltaTime;
                        break;
                    case Speed.Law:
                        // マウスのワールド座標より、進行方向の単位ベクトルを取得する
                        dif = (cWorld - this.transform.position).normalized * (playerSpeed * 0.25f) * Time.deltaTime;
                        break;
                }

                // 移動処理
                transform.position = movePos;
                transform.localEulerAngles = new Vector3(0, angle, 0);

                if (scroll > 0.05f)
                {
                    speed = Speed.High;
                }else if(scroll < -0.05f)
                {
                    speed = Speed.Law;
                }

                if (energy.Value >= 30 && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("true");
                    skillSubject.OnNext(energy.Value);

                    //isSkill = true;
                }

                if (ultimateGage.Value >= 100 && Input.GetMouseButtonDown(1))
                {
                    ultimate.OnNext(ultimateGage.Value);
                }

            }).AddTo(this.gameObject);

        // 0.2秒毎に弾を発射するイベント
        this.UpdateAsObservable()
        .Where(_ => GameManagement.Instance.isPause.Value == false)
        .Sample(TimeSpan.FromSeconds(0.35f))
        .Subscribe(_ =>
        {
            shootSubject.OnNext(BulletList.Normal);
        }).AddTo(this.gameObject);

        GameManagement.Instance.enemyUlt.Where(x => GameManagement.Instance.enemyUlt.Value == true)
        .Subscribe(x =>
        {
            Debug.Log("KEN");
            playerHp.Value = 0;
        }).AddTo(this.gameObject);

        // 衝突判定（弾）
        this.OnTriggerEnterAsObservable()
            .Where(c => c.gameObject.tag == "Bullet")
            .Subscribe(c =>
            {
                BulletManager bullet;
                bullet = c.gameObject.GetComponent<BulletManager>();
                if (bullet.shootChara == BulletManager.ShootChara.Enemy)
                {
                    // ヒットした弾は消滅させる
                    bullet.bulletState.Value = BulletManager.BulletState.Destroy;
                    // 敵による攻撃であればダメージを受ける
                    HitDamage();
                }
            }).AddTo(this.gameObject);

        // 衝突判定（アイテム）
        this.OnTriggerEnterAsObservable()
            .Where(c => c.gameObject.tag == "Item")
            .Subscribe(c =>
            {
                // 衝突したアイテムの情報を取得、各種パラメータに反映
                ItemManager item;
                item = c.gameObject.GetComponent<ItemManager>();

                if (item.itemType == ItemManager.ItemType.Score)
                {
                    GameManagement.Instance.gameScore.Value += item.itemScore;
                    // アイテム取得で必殺技のリキャスト短縮
                    ultimateGage.Value++;
                    // 衝突したアイテムは消滅させる
                    item.poolState.Value = ItemManager.ItemPoolState.Destroy;
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
                        dif = (cWorld - this.transform.position).normalized * refrectPower * Time.deltaTime;

                    }).AddTo(this.gameObject);

                    isBarrier.Where(_ => isBarrier.Value == false)
                    .Subscribe(_ =>
                    {
                        dif = (cWorld - this.transform.position).normalized * refrectPower * Time.deltaTime;
                    }).AddTo(this.gameObject);
                }
            }).AddTo(this.gameObject);

        // スキルストック復活処理
        this.UpdateAsObservable()
            .Where(_ => energy.Value < maxEnergy)
            .Sample(TimeSpan.FromSeconds(10.0f))
            .Subscribe(_ => 
            {
                energy.Value += skillCost;
                // UIにエネルギーを設定
                playerHUD.EnergyRP.Value = energy.Value;
            }).AddTo(this.gameObject);

        // バリア復活処理、3秒毎に回復、被弾直後は起動しない
        this.UpdateAsObservable()
            .Where(_ => status.barrier != maxBarrier && isHit.Value == false)
            .Sample(TimeSpan.FromSeconds(3.0f))
            .Subscribe(_ => 
            {
                status.barrier++;
                playerHUD.BarrierRP.Value = status.barrier;
                if (status.barrier >= maxBarrier)
                {
                    status.barrier = maxBarrier;
                }
            }).AddTo(this.gameObject);

        // 必殺技チャージ処理
        this.UpdateAsObservable()
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Where(_ => ultimateGage.Value <= maxultimateGage)
            .Sample(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => 
            {
                // 1秒ごとに必殺技ゲージ回復
                ultimateGage.Value++;
                playerHUD.UltimateRP.Value = ultimateGage.Value;
            }).AddTo(this.gameObject);

        // 被弾して7秒経過後、バリアが復活するようになる
        isHit.Where(_ => GameManagement.Instance.isPause.Value == false)
            .Sample(TimeSpan.FromSeconds(7.0f))
            .Subscribe(_ =>
            {
                isHit.Value = false;
            }).AddTo(this.gameObject);

        invincible.Where(_ => GameManagement.Instance.isPause.Value == false)
            .Where(_ => invincible.Value == true)
            .Sample(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                invincible.Value = false;
            }).AddTo(this.gameObject);

        playerHp
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Subscribe(_ =>
            {
                if (playerHp.Value >= 1)
                {
                    playerHUD.HealthRP.Value = playerHp.Value;
                }
                else
                {
                    GameManagement.Instance.gameOver.Value = true;
                }
                
            }).AddTo(this.gameObject);
    }

    public void HitDamage()
    {
        isHit.Value = true;
        GameManagement.Instance.enemyDeathCombo.Value = 0;
        if(status.barrier >= 1)
        {
            status.barrier--;
            playerHUD.BarrierRP.Value = status.barrier;
        }else if(invincible.Value == false)
        {
            playerHp.Value--;
            playerHUD.HealthRP.Value = playerHp.Value;
            FlashSubject.OnNext(0.5f);
            invincible.Value = true;
            GameManagement.Instance.combo.Value = 0;
        }
    }
}

/*
         // レベルの取得
        level.Value = GameManagement.Instance.playerLevel.Value;
        // プレイヤーのパラメータのデータリストを取得
        dataList = Resources.Load<EnemyDataList>(string.Format("PlayerData"));
        // データリストよりレベルに応じたパラメータを取得
        status = dataList.EnemyStatusList[level.Value - 1];
                // 最大HPの設定
        maxHp.Value = playerHp.Value;
        // UI上の最大HP設定
        playerHUD.maxHealth = maxHp.Value;
        // 最大バリアの設定
        maxBarrier = playerBarrier.Value;
        // エネルギーの設定
        energy.Value = 0;
        // 最大エネルギーの設定
        maxEnergy = status.energy;
        // UI上の最大エネルギーの設定
        playerHUD.maxEnergy = maxEnergy;
        // UI上の最大バリア設定
        playerHUD.maxBarrier = maxBarrier;
        // UI上の最大必殺技ゲージの設定
        playerHUD.maxUltimate = maxultimateGage;
        // スキルの設定
        skillStock.Value = 1;
     
     */
