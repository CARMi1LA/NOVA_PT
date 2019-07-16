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
    [SerializeField] public float shootSpeed;            // 発射スピード
    [SerializeField] private float rangeLimit;           // 最大距離
    [SerializeField] public int damageAtk;               // 攻撃力
    [SerializeField] public int listIndex;               // （デバッグ用）消滅時にスポナーに消滅を知らせるために必要

    [SerializeField] public Transform shootOriginTrans; // 発射元の座標
    // Start is called before the first frame update
    void Start()
    {
        // 弾を発射する
        this.GetComponent<Rigidbody>().AddForce(shootOriginTrans.forward * shootSpeed, ForceMode.Impulse);
        // 向きを発射方向に向ける
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward, shootOriginTrans.forward);
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
    public void BulletCreate(int atk, float speed, Transform origin, ShootChara chara, int index)
    {
        // このオブジェクトを表示する
        this.gameObject.SetActive(true);
        // スポナーの生成数を増やす
        BulletSpawner.Instance.bulletCount++;
        // ステートの初期化
        bulletState = BulletState.Active;
        // 攻撃力の設定
        damageAtk = atk;
        // 発射スピードの設定
        shootSpeed = speed;
        // 発射元座標の設定
        shootOriginTrans = origin;
        // 発射元キャラクターの設定
        shootChara = chara;
        // 自分の要素番号の設定
        listIndex = index;
        // 現在の座標の初期化
        this.transform.position = origin.position;
    }

    // カメラの範囲外に到達したら削除される
    private void OnBecameInvisible()
    {
        bulletState = BulletState.Destroy;
        BulletDestroy();
    }
}
