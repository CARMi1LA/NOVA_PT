using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerManager : MonoBehaviour,IDamage
{
    [SerializeField] EnemyDataList dataList;
    [SerializeField] EnemyStatus status;

    [SerializeField] private IntReactiveProperty hp;              // 現在のHP
    [SerializeField] private IntReactiveProperty maxHp;           // 最大HP
    [SerializeField] private IntReactiveProperty energy;          // エネルギー

    [SerializeField] private Rigidbody playerRigid;

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
    // スコアを監視可能な変数
    public IntReactiveProperty score = new IntReactiveProperty(0);

    void Awake()
    {
        // レベルの取得
        level = GameManagement.Instance.playerLevel;
        // プレイヤーのパラメータのデータリストを取得
        dataList = Resources.Load<EnemyDataList>(string.Format("PlayerData"));
        // データリストよりレベルに応じたパラメータを取得
        status = dataList.EnemyStatusList[level.Value - 1];
        // HPの設定
        hp.Value = status.hp;
        // 最大HPの設定
        maxHp.Value = status.hp;
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
                cScreen.z = 100.0f;
                // スクリーン座標からワールド座標へ変換
                cWorld = Camera.main.ScreenToWorldPoint(cScreen);

                // マウスのワールド座標より、進行方向の単位ベクトルを取得する
                dif = (cWorld - this.transform.position).normalized;
                // 単位ベクトルの方向に移動する、Y軸は常に0に
                Vector3 movePos = this.transform.position + dif;
                movePos.y = 0.0f;

                // 移動処理
                transform.position = movePos;

                if (Input.GetMouseButton(0))
                {
                    GameManagement.Instance.onClick.Value = true;
                    movePos = Vector3.zero;
                }
                else
                {
                    GameManagement.Instance.onClick.Value = false;
                }
            }).AddTo(this.gameObject);

        // 衝突判定（弾）
        this.OnTriggerEnterAsObservable()
            .Where(c => gameObject.tag == "Bullet")
            .Subscribe(c =>
            {
                BulletManager bullet;
                bullet = c.gameObject.GetComponent<BulletManager>();
                if (bullet.shootChara == BulletManager.ShootChara.Player)
                {
                    // プレイヤーによる攻撃であればダメージを受ける
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
    }

    public void HitDamage()
    {
    }
}
