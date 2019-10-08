using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class PlayerManager : MonoBehaviour,IDamage
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

    [SerializeField] private IntReactiveProperty hp;              // 現在のHP
    [SerializeField] private IntReactiveProperty maxHp;           // 最大HP
    [SerializeField] private IntReactiveProperty energy;          // エネルギー
    [SerializeField] private IntReactiveProperty ultimateGage;     // 必殺技ゲージ

    [SerializeField] private Rigidbody playerRigid;               // プレイヤーのRigidbody
    [SerializeField] Speed speed = Speed.Law;                     // 速度モード

    [SerializeField] private int maxBarrier;                      // 最大バリア数
    [SerializeField] private int maxSkillStock = 3;               // 最大スキルストック数
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
    // 子機のトランスフォーム
    [SerializeField] private Transform bitRight,bitLeft;
    // スコアを監視可能な変数
    public IntReactiveProperty score = new IntReactiveProperty(0);
    // スキルストック数
    private IntReactiveProperty skillStock = new IntReactiveProperty(0);
    // 被弾直後かどうか
    private BoolReactiveProperty isHit = new BoolReactiveProperty(false);

    [SerializeField] private ParticleSystem  deathPS,ultPS;

    void Awake()
    {
        // レベルの取得
        level.Value = GameManagement.Instance.playerLevel.Value;
        // プレイヤーのパラメータのデータリストを取得
        dataList = Resources.Load<EnemyDataList>(string.Format("PlayerData"));
        // データリストよりレベルに応じたパラメータを取得
        status = dataList.EnemyStatusList[level.Value - 1];
        // HPの設定
        hp.Value = status.hp;
        // 最大HPの設定
        maxHp.Value = status.hp;
        // UI上の最大HP設定
        playerHUD.maxHealth = maxHp.Value;
        // 最大バリアの設定
        maxBarrier = status.barrier;
        // エネルギーの設定
        energy.Value = status.energy;
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

        // UIにHPを設定
        playerHUD.HealthRP.Value = hp.Value;
        // UIにバリアを設定
        playerHUD.BarrierRP.Value = status.barrier;
        // UIにエネルギーを設定
        playerHUD.EnergyRP.Value = energy.Value / 2;
        // UIにULTゲージを表示
        playerHUD.UltimateRP.Value = ultimateGage.Value;
    }

    // Start is called before the first frame update
    void Start()
    {
        // レベルが更新された時のみ、呼び出される
        level.Subscribe(_ =>
        {
            // パラメータの更新
            status = dataList.EnemyStatusList[level.Value - 1];
            // 最大HPの更新
            maxHp.Value = status.hp;
            // レベルアップ分のHPを現在のHPに代入
            hp.Value = hp.Value + (hp.Value - maxHp.Value);
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

                if (energy.Value >= skillCost && Input.GetMouseButtonDown(0) == true)
                {
                    energy.Value -= skillCost;
                    playerHUD.EnergyRP.Value = energy.Value;
                }

                // 必殺技処理
                ultimateGage
                    .Where(u => ultimateGage.Value >= maxultimateGage)
                    .Where(u => Input.GetMouseButtonDown(1) == true)
                    .Subscribe(u =>
                    {
                        Instantiate(ultPS, this.transform.position, Quaternion.identity);
                        GameManagement.Instance.playerUlt.Value = true;
                    }).AddTo(this.gameObject);
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
        .Where(_ => GameManagement.Instance.isPause.Value == false)
        .Sample(TimeSpan.FromSeconds(0.20f))
        .Subscribe(_ =>
        {
            new BulletData(20.0f, bitLeft.transform, BulletManager.ShootChara.Player, 0, 0.0f,angle);
            new BulletData(20.0f, bitRight.transform, BulletManager.ShootChara.Player, 0, 0.0f,angle);
        }).AddTo(this.gameObject);

        energy.Where(_ => GameManagement.Instance.isPause.Value == false)
            .Subscribe(_ => 
            {
                Debug.Log("kenSkill");
                new BulletData(25.0f, bitLeft.transform, BulletManager.ShootChara.Player, 0, 0.0f, angle);
                new BulletData(25.0f, bitRight.transform, BulletManager.ShootChara.Player, 0, 0.0f, angle);
            }).AddTo(this.gameObject);

        GameManagement.Instance.enemyUlt.Where(x => x == GameManagement.Instance.enemyUlt.Value == true)
        .Subscribe(x =>
        {
            hp.Value = 0;
        }).AddTo(this.gameObject);

        // 衝突判定（弾）
        this.OnTriggerEnterAsObservable()
            .Where(c => gameObject.tag == "Bullet")
            .Subscribe(c =>
            {
                BulletManager bullet;
                bullet = c.gameObject.GetComponent<BulletManager>();
                if (bullet.shootChara == BulletManager.ShootChara.Enemy)
                {
                    // 敵による攻撃であればダメージを受ける
                    HitDamage();
                    // ヒットした弾は消滅させる
                    bullet.BulletDestroy();
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
                    item.ItemDestroy();
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
        isHit.Where(_ => true)
            .Sample(TimeSpan.FromSeconds(7.0f))
            .Subscribe(_ =>
            {
                isHit.Value = false;
            }).AddTo(this.gameObject);
        hp.Subscribe(_ =>
        {
            playerHUD.HealthRP.Value = hp.Value;
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
        }else
        {
            hp.Value--;
            playerHUD.HealthRP.Value = hp.Value;
            GameManagement.Instance.combo.Value = 0;
        }
    }
}
