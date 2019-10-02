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

    [SerializeField] private IntReactiveProperty hp;              // 現在のHP
    [SerializeField] private IntReactiveProperty maxHp;           // 最大HP
    [SerializeField] private IntReactiveProperty energy;          // エネルギー

    [SerializeField] private Rigidbody playerRigid;               // プレイヤーのRigidbody
    [SerializeField] Speed speed = Speed.Law;                     // 速度モード

    [SerializeField] private int maxBarrier;                      // 最大バリア数
    [SerializeField] private int maxSkillStock = 3;               // 最大スキルストック数
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
    // 子機のトランスフォーム
    [SerializeField] private Transform bitRight,bitLeft;
    // スコアを監視可能な変数
    public IntReactiveProperty score = new IntReactiveProperty(0);
    // スキルストック数
    private IntReactiveProperty skillStock = new IntReactiveProperty(0);
    // 被弾直後かどうか
    private BoolReactiveProperty isHit = new BoolReactiveProperty(false);

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
        // 最大バリアの設定
        maxBarrier = status.barrier;
        // スキルの設定
        skillStock.Value = 1;
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
            .Subscribe(_ =>
            {
                // マウスのスクリーン座標を取得
                cScreen = Input.mousePosition;
                // カメラの焦点を補正
                cScreen.z = 25.0f;
                // スクリーン座標からワールド座標へ変換
                cWorld = Camera.main.ScreenToWorldPoint(cScreen);
                // マウスホイールの回転量取得
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                
                // 単位ベクトルの方向に移動する、Y軸は常に0に
                Vector3 movePos = this.transform.position + dif;
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

                if (scroll > 0.05f)
                {
                    speed = Speed.High;
                }else if(scroll < -0.05f)
                {
                    speed = Speed.Law;
                }
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Sample(TimeSpan.FromSeconds(0.20f))
            .Subscribe(_ => 
            {
                new BulletData(25.0f, bitRight, BulletManager.ShootChara.Player, 0, 0.0f);
                new BulletData(25.0f, bitLeft, BulletManager.ShootChara.Player, 0, 0.0f);
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
            });

        // 衝突判定（アイテム）
        this.OnTriggerEnterAsObservable()
            .Where(c => c.gameObject.tag == "Item")
            .Subscribe(c =>
            {
                // 衝突したアイテムの情報を取得、各種パラメータに反映
                ItemManager item;
                item = c.gameObject.GetComponent<ItemManager>();

                score.Value += item.itemScore;
                hp.Value += item.itemLife;
                energy.Value += item.itemEnergy;
                // 衝突したアイテムは消滅させる
                item.ItemDestroy();
            });

        // スキルストック復活処理
        this.UpdateAsObservable()
            .Where(_ => skillStock.Value < maxSkillStock)
            .Sample(TimeSpan.FromSeconds(10.0f))
            .Subscribe(_ => 
            {
                skillStock.Value++;
            }).AddTo(this.gameObject);

        // バリア復活処理、3秒毎に回復、被弾直後は起動しない
        this.UpdateAsObservable()
            .Where(_ => status.barrier != maxBarrier && isHit.Value == false)
            .Sample(TimeSpan.FromSeconds(3.0f))
            .Subscribe(_ => 
            {
                status.barrier++;
                if (status.barrier >= maxBarrier)
                {
                    status.barrier = maxBarrier;
                }
            }).AddTo(this.gameObject);

        // 被弾して7秒経過後、バリアが復活するようになる
        isHit.Where(_ => true)
            .Sample(TimeSpan.FromSeconds(7.0f))
            .Subscribe(_ =>
            {
                isHit.Value = false;
            }).AddTo(this.gameObject);
    }

    public void HitDamage()
    {
        isHit.Value = true;
        if(status.barrier >= 1)
        {
            status.barrier--;
        }else
        {
            hp.Value--;
            GameManagement.Instance.combo.Value = 0;
        }
    }
}
