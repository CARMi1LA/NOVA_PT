using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

using Random = UnityEngine.Random;

public class BulletManager : MonoBehaviour
{
    /* 
  弾が持つスクリプト
  生成時のステータスを元に実行される
  敵やプレイヤーに衝突したらこのスクリプトのステータスを元にダメージ処理を行う
    */

    // 誰が攻撃したか
    public enum ShootChara
    {
        Player = 0,
        Enemy,
        None
    }

    // 弾の生成状況
    public enum BulletState
    {
        Active = 0,     // 生成済み
        Destroy,        // 消滅
        Pool            // 生成待機状態
    }

    [SerializeField] public ShootChara shootChara;       // 生成元のキャラクター
    [SerializeField] private AIListManager.AtkList bulletType; // 弾の種類
    [SerializeField] public float shootSpeed;            // 発射スピード
    [SerializeField] private float rangeLimit;           // 最大距離
    [SerializeField] private float originAngle;          // 発射元の角度
    [SerializeField] private float bulletDistance;       // 
    [SerializeField] private float horizontalOffset;
    [SerializeField] private float verticalOffset;

    [SerializeField] public Transform shootOriginTrans;  // 発射元の座標
    [SerializeField] public Transform playerTrans;
    [SerializeField] public Vector3 moveFoward;
    [SerializeField] public Quaternion bulletRot;        // 弾の角度

    [SerializeField] public BulletStateReactiveProperty bulletState = new BulletStateReactiveProperty();

    // 初期化用Subject
    Subject<Unit> bulletInit = new Subject<Unit>();
    // 
    Subject<float> shootBoost = new Subject<float>();

    // 参照用のカスタムプロパティ
    [SerializeField]
    public IReadOnlyReactiveProperty<BulletState> stateProperty
    {
        get { return bulletState; }
    }
    // Start is called before the first frame update
    void Start()
    {
        // プレイヤー座標の取得
        playerTrans = GameManagement.Instance.playerTrans;
        // 弾の向きを調整
        moveFoward = (GameManagement.Instance.cWorld - this.transform.position).normalized;

        // 初期化処理
        bulletInit.Subscribe(_ => 
        {
            // 進行方向の初期化
            moveFoward = Vector3.zero;
            // 回転角度の初期化
            bulletRot = Quaternion.identity;
            // 弾速の初期化
            shootSpeed = 0.0f;
            // 発射角度の初期化
            originAngle = 0.0f;
            // 二点間距離の初期化
            bulletDistance = 0.0f;
            // 生成元座標の初期化
            shootOriginTrans = null;
            // 速度の初期化
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // 座標の初期化
            this.transform.position = Vector3.zero;
            // 発射キャラクター識別情報の初期化
            shootChara = ShootChara.None;
            // 再生成待機モードに移行
            bulletState.Value = BulletState.Pool;
        }).AddTo(this.gameObject);

        // 加速モードの処理（最初の3秒は通常速度の1/4、以降は10倍の速度に
        shootBoost.Do(_ => shootSpeed = shootSpeed * 0.25f)
            .Delay(TimeSpan.FromSeconds(3.0f))
            .Subscribe(val => 
            {
                shootSpeed = val * 10.0f;
            }).AddTo(this.gameObject);

        // 発射元のキャラクターが敵の場合
        if (shootChara == ShootChara.Enemy)
        {
            playerTrans = GameManagement.Instance.playerTrans;
            moveFoward = (playerTrans.position - this.transform.position).normalized;
            this.transform.forward = moveFoward;
            // 弾を発射する。弾の種類ごとに移動量や角度を設定する
            switch (bulletType)
            {
                // 低加速モード、3秒間通常よりも低速で移動しその後通常の2倍の弾速で移動
                case AIListManager.AtkList.Booster:
                    shootBoost.OnNext(shootSpeed);
                    break;
                case AIListManager.AtkList.Forrow:
                    if (shootChara == ShootChara.Enemy)
                    {
                        var diff = playerTrans.position - this.transform.position;
                        float hitTime = 3.0f;
                        Vector3 accel = Vector3.zero;
                        Vector3 velocity = Vector3.zero;

                        accel += (diff - velocity * hitTime) * 2.0f / (hitTime * hitTime);
                        if (accel.magnitude > 100.0f)
                        {
                            hitTime -= Time.deltaTime;
                        }
                        velocity += accel * Time.deltaTime;
                        this.GetComponent<Rigidbody>().velocity = this.transform.position + velocity * Time.deltaTime;
                    }
                    else if (shootChara == ShootChara.Player)
                    {
                    }
                    break;
                default:
                    //shootSpeed *= 1.25f + Time.deltaTime;
                    break;
            }
        }
        // 発射元のキャラクターがプレイヤーの場合
        else if (shootChara == ShootChara.Player)
        {
            playerTrans = GameManagement.Instance.playerTrans;
            moveFoward = (GameManagement.Instance.cWorld - this.transform.position).normalized;
            float radian = originAngle * Mathf.Deg2Rad;
            Vector3 foward = new Vector3(Mathf.Cos(radian), 0.0f, Mathf.Sin(radian));
        }
        this.UpdateAsObservable()
            .Where(_ => stateProperty.Value == BulletState.Active)
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Subscribe(_ => 
            {
                // 移動処理
                this.GetComponent<Rigidbody>().velocity = this.transform.forward * (shootSpeed * Time.deltaTime);
                // 二点間距離の更新
                bulletDistance = (this.transform.position - shootOriginTrans.position).sqrMagnitude;
            }).AddTo(this.gameObject);

        // 最大距離を超えたら消滅する
        this.UpdateAsObservable()
            .Where(_ => stateProperty.Value == BulletState.Active)
            .Where(_ => bulletDistance >= rangeLimit)
            .Subscribe(_ => 
            {
                bulletState.Value = BulletState.Destroy;
            }).AddTo(this.gameObject);

        GameManagement.Instance.playerUlt.Where(_ => GameManagement.Instance.isPause.Value == true)
            .Where(_ => _ == true)
            .Where(_ => shootChara == ShootChara.Enemy)
            .Subscribe(_ => 
            {
                Debug.Log("Destroy1");
                bulletState.Value = BulletState.Destroy;
            }).AddTo(this.gameObject);

        GameManagement.Instance.enemyUlt.Where(_ => GameManagement.Instance.isPause.Value == true)
        .Where(_ => _ == true)
        .Where(_ => shootChara == ShootChara.Player)
        .Subscribe(_ =>
        {
            Debug.Log("Destroy2");
            bulletState.Value = BulletState.Destroy;
        }).AddTo(this.gameObject);

        stateProperty.Where(_ => stateProperty.Value == BulletState.Destroy)
            .Subscribe(_ => 
            {
                Debug.LogFormat("Destroy{0}", shootOriginTrans.name);
                // 現在の生成数を減らす
                BulletSpawner.Instance.bulletCount.Value--;
                // 弾のスポナーに削除情報を送る
                BulletSpawner.Instance.BulletRemove(this);
                // 弾情報の初期化
                bulletInit.OnNext(Unit.Default);
                // このオブジェクトを非表示にする
                this.gameObject.SetActive(false);
            }).AddTo(this.gameObject);
    }

    // 発射する方向を設定するメソッド
    public void SetBulletRot(Quaternion rot)
    {
        bulletRot = rot;
    }

    // 弾生成処理
    public void BulletCreate(float speed, Transform origin, ShootChara chara, float rot, AIListManager.AtkList type,float angle)
    {
        // 弾の見た目の設定
        switch (shootChara)
        {
            case ShootChara.Player:
                break;
            case ShootChara.Enemy:
                break;
        }
        // このオブジェクトを表示する
        this.gameObject.SetActive(true);
        // スポナーの生成数を増やす
        BulletSpawner.Instance.bulletCount.Value++;
        // ステートの初期化
        bulletState.Value = BulletState.Active;
        // 弾の種類の設定
        bulletType = type;
        // 発射スピードの設定
        shootSpeed = speed;
        // 発射元座標の設定
        shootOriginTrans = origin;
        // 発射元キャラクターの設定
        shootChara = chara;
        // 角度の設定
        originAngle = angle;
        // 現在の座標の初期化
        this.transform.position = origin.position;
        // 発射角度の設定
        this.transform.localEulerAngles = new Vector3(0.0f, originAngle, 0.0f);
        // 発射角度の設定
        this.SetBulletRot(Quaternion.AngleAxis(rot, Vector3.up));
    }
    // プール返却前に行う初期化処理
    void BulletInit()
    {
        // 進行方向の初期化
        moveFoward = Vector3.zero;
        // 回転角度の初期化
        bulletRot = Quaternion.identity;
        // 弾速の初期化
        shootSpeed = 0.0f;
        // 発射角度の初期化
        originAngle = 0.0f;
        // 二点間距離の初期化
        bulletDistance = 0.0f;
        // 生成元座標の初期化
        shootOriginTrans = null;
        // 速度の初期化
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // 座標の初期化
        this.transform.position = Vector3.zero;
        // 発射キャラクター識別情報の初期化
        shootChara = ShootChara.None;
        // 再生成待機モードに移行
        bulletState.Value = BulletState.Pool;

    }

    // カメラの範囲外に到達したら削除される
    private void OnBecameInvisible()
    {
        bulletState.Value = BulletState.Destroy;
    }
}

// 敵AI専用のカスタムプロパティ
[System.Serializable]
public class BulletStateReactiveProperty : ReactiveProperty<BulletManager.BulletState>
{
    public BulletStateReactiveProperty() { }
    public BulletStateReactiveProperty(BulletManager.BulletState initialValue) : base(initialValue) { }
}

// 消滅処理
// 弾を発射する（旧バージョン）
// this.GetComponent<Rigidbody>().AddForce((shootOriginTrans.forward) * shootSpeed, ForceMode.Impulse);
//public void BulletDestroy()
//{
//    // ステートが消滅状態なら実行
//    if (bulletState == BulletState.Destroy)
//    {
//        Debug.LogFormat("Destroy{0}", shootOriginTrans.name);
//        // 現在の生成数を減らす
//        BulletSpawner.Instance.bulletCount.Value--;
//        // 弾のスポナーに削除情報を送る
//        BulletSpawner.Instance.BulletRemove(this);
//        // 弾情報の初期化
//        BulletInit();
//        // このオブジェクトを非表示にする
//        this.gameObject.SetActive(false);
//    }

//this.transform.position += shootOriginTrans.forward * shootSpeed * Time.deltaTime;
//movePos = transform.forward * shootSpeed;
//this.GetComponent<Rigidbody>().AddForce((shootOriginTrans.forward) * shootSpeed, ForceMode.Impulse);
// 弾を発射する（新規）
//this.GetComponent<Rigidbody>().velocity = bulletRot * shootOriginTrans.forward * shootSpeed * Time.deltaTime;

//}