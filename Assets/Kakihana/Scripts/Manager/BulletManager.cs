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
    [SerializeField] public int listIndex;               // （デバッグ用）消滅時にスポナーに消滅を知らせるために必要

    [SerializeField] public Transform shootOriginTrans;  // 発射元の座標
    [SerializeField] public Transform playerTrans;
    [SerializeField] public Vector3 movePos;
    [SerializeField] public Quaternion bulletRot;        // 弾の角度
    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameManagement.Instance.playerTrans;
        this.transform.position = shootOriginTrans.position;
        this.UpdateAsObservable()
            .Where(_ => bulletState == BulletState.Active)
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Subscribe(_ => 
            {
                Debug.Log("ken1");
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
                        float hitTime = 3.0f;
                        Vector3 accel = Vector3.zero;
                        Vector3 velocity = Vector3.zero;
                        var diff = playerTrans.position - this.transform.position;
                        accel += (diff - velocity * hitTime) * 2.0f / (hitTime * hitTime);
                        if (accel.magnitude > 100.0f)
                        {
                            hitTime -= Time.deltaTime;
                        }
                        velocity += accel * Time.deltaTime;
                        this.GetComponent<Rigidbody>().velocity = this.transform.position + velocity * Time.deltaTime;
                        break;
                    default:
                        Debug.Log("ken2");
                        //movePos = transform.forward * shootSpeed;
                        //this.GetComponent<Rigidbody>().AddForce((shootOriginTrans.forward) * shootSpeed, ForceMode.Impulse);
                        // 弾を発射する（新規）
                        //this.GetComponent<Rigidbody>().velocity = bulletRot * shootOriginTrans.forward * shootSpeed * Time.deltaTime;
                        break;
                }
                //this.transform.position += shootOriginTrans.forward * shootSpeed * Time.deltaTime;
                // 弾を発射する（旧バージョン）
                // this.GetComponent<Rigidbody>().AddForce((shootOriginTrans.forward) * shootSpeed, ForceMode.Impulse);
                // 向きを発射方向に向ける
                movePos.y = 0.0f;
                this.transform.position += Vector3.forward * shootSpeed * Time.deltaTime;
                Debug.Log(shootOriginTrans.forward);
                //this.transform.rotation = Quaternion.LookRotation(this.transform.forward, shootOriginTrans.forward);
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
            // 現在の生成数を減らす
            BulletSpawner.Instance.bulletCount--;
            // 弾のスポナーに削除情報を送る
            BulletSpawner.Instance.BulletRemove(this);
            // このオブジェクトを非表示にする
            this.gameObject.SetActive(false);
            // 再生成待機モードに移行
            bulletState = BulletState.Pool;
        }

    }

    // 弾生成処理
    public void BulletCreate(float speed, Transform origin, ShootChara chara, float rot, AIListManager.AtkList type)
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
        BulletSpawner.Instance.bulletCount++;
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
        // 現在の座標の初期化
        this.transform.position = origin.position;
        // 発射角度の設定
        this.SetBulletRot(Quaternion.AngleAxis(rot, Vector3.up));
    }

    // カメラの範囲外に到達したら削除される
    private void OnBecameInvisible()
    {
        bulletState = BulletState.Destroy;
        BulletDestroy();
    }
}
