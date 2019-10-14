using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

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
    [SerializeField] private BulletState bulletState;    // 弾の生成状況
    [SerializeField] private AIListManager.AtkList bulletType; // 弾の種類
    [SerializeField] public float shootSpeed;            // 発射スピード
    [SerializeField] private float rangeLimit;           // 最大距離
    [SerializeField] private float originAngle;          // 発射元の角度
    [SerializeField] private float horizontalOffset;
    [SerializeField] private float verticalOffset;

    [SerializeField] public Transform shootOriginTrans;  // 発射元の座標
    [SerializeField] public Transform playerTrans;
    [SerializeField] public Vector3 moveFoward;
    [SerializeField] public Quaternion bulletRot;        // 弾の角度
    // Start is called before the first frame update
    void Start()
    {
        
        playerTrans = GameManagement.Instance.playerTrans;
        moveFoward = (GameManagement.Instance.cWorld - this.transform.position).normalized;

        if (shootChara == ShootChara.Enemy)
        {
            playerTrans = GameManagement.Instance.playerTrans;
            moveFoward = (playerTrans.position - this.transform.position).normalized;
            this.transform.forward = moveFoward;
            Debug.Log(this.transform.forward);
        }
        else if(shootChara == ShootChara.Player)
        {
            playerTrans = GameManagement.Instance.playerTrans;
            moveFoward = (GameManagement.Instance.cWorld - this.transform.position).normalized;
            float radian = originAngle * Mathf.Deg2Rad;
            Vector3 foward = new Vector3(Mathf.Cos(radian), 0.0f, Mathf.Sin(radian));
        }
        this.UpdateAsObservable()
            .Where(_ => bulletState == BulletState.Active)
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Subscribe(_ => 
            {
                if (shootChara == ShootChara.None)
                {
                    bulletState = BulletState.Destroy;
                    BulletDestroy();
                }
                // 弾を発射する。弾の種類ごとに移動量や角度を設定する
                switch (bulletType)
                {
                    // 低加速モード、3秒間通常よりも低速で移動しその後通常の2倍の弾速で移動
                    case AIListManager.AtkList.Booster:
                        float time = 0.0f;
                        shootSpeed *= 0.1f;
                        time = Time.deltaTime;
                        if (time >= 3.0f)
                        {
                            shootSpeed *= 20.0f;
                        }
                        // 弾を発射する（新規）
                        this.GetComponent<Rigidbody>().velocity = bulletRot * shootOriginTrans.forward * shootSpeed;
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
                        }else if(shootChara == ShootChara.Player)
                        {
                            //PlayerManager pm = playerTrans.gameObject.GetComponent<PlayerManager>();
                            //var targetDis = 0.0f;
                            //GameObject targetObj = null;
                            //foreach (var item in pm.apprEnemys)
                            //{
                            //    var dis = (item.transform.position - this.transform.position).sqrMagnitude;
                            //    if (targetDis == 0)
                            //    {
                            //        targetDis = dis;
                            //        targetObj = item;
                            //    }
                            //    if (targetDis >= dis)
                            //    {
                            //        targetDis = dis;
                            //        targetObj = item;
                            //    }
                            //}
                            //Vector3 radian = (targetObj.transform.position - this.transform.position).normalized;
                            //float angle = Mathf.Atan2(radian.z, radian.x);
                            //this.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(this.transform.rotation.y, angle, 0.25f), 0);
                            //shootSpeed *= 1.5f + Time.deltaTime;
                        }
                        break;
                    default:
                        shootSpeed *= 1.25f + Time.deltaTime;
                        //this.transform.position += shootOriginTrans.forward * shootSpeed * Time.deltaTime;
                        //movePos = transform.forward * shootSpeed;
                        //this.GetComponent<Rigidbody>().AddForce((shootOriginTrans.forward) * shootSpeed, ForceMode.Impulse);
                        // 弾を発射する（新規）
                        //this.GetComponent<Rigidbody>().velocity = bulletRot * shootOriginTrans.forward * shootSpeed * Time.deltaTime;
                        break;
                }
                //　カメラのビューポートの限界位置をオフセット位置を含めて計算

                this.GetComponent<Rigidbody>().velocity = this.transform.forward * shootSpeed;
                // 弾を発射する（旧バージョン）
                // this.GetComponent<Rigidbody>().AddForce((shootOriginTrans.forward) * shootSpeed, ForceMode.Impulse);
            }).AddTo(this.gameObject);

        // 最大距離を超えたら消滅する
        this.UpdateAsObservable()
            .Where(_ => bulletState == BulletState.Active)
            .Where(_ => Vector3.Distance(this.transform.position, shootOriginTrans.position) >= rangeLimit)
            .Subscribe(_ =>
            {
                bulletState = BulletState.Destroy;
                BulletDestroy();
            }).AddTo(this.gameObject);

        GameManagement.Instance.playerUlt.Where(_ => GameManagement.Instance.isPause.Value == true)
            .Where(_ => true)
            .Where(_ => shootChara == ShootChara.Enemy)
            .Subscribe(_ => 
            {
                bulletState = BulletState.Destroy;
                BulletDestroy();
            }).AddTo(this.gameObject);

        GameManagement.Instance.enemyUlt.Where(_ => GameManagement.Instance.isPause.Value == true)
        .Where(_ => true)
        .Where(_ => shootChara == ShootChara.Player)
        .Subscribe(_ =>
        {
            bulletState = BulletState.Destroy;
            BulletDestroy();
        }).AddTo(this.gameObject);
    }

    // 発射する方向を設定するメソッド
    public void SetBulletRot(Quaternion rot)
    {
        bulletRot = rot;
    }

    // 消滅処理
    public void BulletDestroy()
    {
        // ステートが消滅状態なら実行
        if (bulletState == BulletState.Destroy)
        {
            // 弾情報の初期化
            BulletInit();
            // 現在の生成数を減らす
            BulletSpawner.Instance.bulletCount.Value--;
            // 弾のスポナーに削除情報を送る
            BulletSpawner.Instance.BulletRemove(this);
            // このオブジェクトを非表示にする
            this.gameObject.SetActive(false);
        }

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
        bulletState = BulletState.Active;
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
        // 生成元座標の初期化
        shootOriginTrans = null;
        // 速度の初期化
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // 座標の初期化
        this.transform.position = Vector3.zero;
        // 発射キャラクター識別情報の初期化
        shootChara = ShootChara.None;
        // 再生成待機モードに移行
        bulletState = BulletState.Pool;

    }
    private void OnEnable()
    {
        
    }

    // カメラの範囲外に到達したら削除される
    private void OnBecameInvisible()
    {
        bulletState = BulletState.Destroy;
        BulletDestroy();
    }
}
