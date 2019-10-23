using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BulletSpawner : BSSingleton<BulletSpawner>
{
    // 弾生成管理クラス

    public AIListManager aiList;
    [SerializeField] AIListManager.AtkList atkList;

    [SerializeField] private int bulletValueMax;            // 弾の最大生成数
    [SerializeField] public IntReactiveProperty bulletCount = new IntReactiveProperty(0); // 弾の生成数
    [SerializeField] private BulletManager[] bulletObj;     // 弾のプレハブ
    [SerializeField] private BulletPool bulletPool;         // 弾のオブジェクトプールクラス

    [SerializeField] private Transform bulletPoolTrans;     // オブジェクトプールを格納するための変数

    // 生成予定の弾のデータを格納するリスト
    [SerializeField] public ReactiveCollection<BulletData> bulletDataList = new ReactiveCollection<BulletData>();

    // 生成した弾を格納するリスト
    [SerializeField] public ReactiveCollection<BulletManager> bulletList = new ReactiveCollection<BulletManager>();
    // Start is called before the first frame update
    void Start()
    {
        // プールの初期化
        bulletPool = new BulletPool(bulletObj[0], bulletPoolTrans);

        // 弾生成処理、生成予定のデータリストに情報が追加された時に動作
        bulletDataList.ObserveAdd()
        .Where(_ => bulletCount.Value <= bulletValueMax)
        .Subscribe(_ =>
        {
            // プールの生成
            var bullet = bulletPool.Rent();
            // 各弾を生成
            bullet.BulletCreate(_.Value.bulletSpeed, _.Value.Origintrans, _.Value.shootChara, _.Value.bulletRot, _.Value.bulletType,_.Value.bulletAngle);
            // 生成済みリスト情報を追加
            bulletList.Add(bullet);
            // 弾生成用データリストは不要になるので破棄する。
            bulletDataList.Remove(_.Value);
        }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => bulletDataList.Count >= bulletValueMax * 0.75f).Subscribe(_ =>
            {
                bulletDataList.Clear();
            }).AddTo(this.gameObject);
    }

    // プールの返却と出現済みリストから弾の情報を削除するメソッド
    public void BulletRemove(BulletManager bm)
    {
        bulletPool.Return(bm);
        bulletList.Remove(bm);
    }
}

public class BulletData
{
    public float bulletSpeed;     // 速度
    public float bulletRot;       // 弾の回転角度
    public float bulletAngle;     // 弾の発射角度
    public Transform Origintrans; // 発射元の座標

    public BulletManager.ShootChara shootChara; // 誰が発射したか
    public AIListManager.AtkList bulletType;    // 弾の種類
    // パラメータの設定
    public BulletData(float speed, Transform trans, BulletManager.ShootChara chara,int actID,float rot,float angle)
    {
        bulletSpeed = speed;
        Origintrans = trans;
        shootChara = chara;
        bulletRot = rot;
        bulletAngle = angle;

        switch (actID)
        {
            case (int)AIListManager.AtkList.Normal:
                bulletType = AIListManager.AtkList.Normal;
                break;
            case (int)AIListManager.AtkList.Scatter:
                bulletType = AIListManager.AtkList.Scatter;
                break;
            case (int)AIListManager.AtkList.Fireworks:
                bulletType = AIListManager.AtkList.Fireworks;
                break;
            case (int)AIListManager.AtkList.Booster:
                bulletType = AIListManager.AtkList.Booster;
                break;
            case (int)AIListManager.AtkList.None:
                break;
            case (int)AIListManager.AtkList.Whirlpool:
                bulletType = AIListManager.AtkList.Whirlpool;
                break;
            case (int)AIListManager.AtkList.Forrow:
                bulletType = AIListManager.AtkList.Forrow;
                break;
            case (int)AIListManager.AtkList.WhirlScatterCombo:
                bulletType = AIListManager.AtkList.WhirlScatterCombo;
                break;
            case (int)AIListManager.AtkList.FireworksCombo:
                bulletType = AIListManager.AtkList.FireworksCombo;
                break;
            case (int)AIListManager.AtkList.UltMegaFireworks:
                bulletType = AIListManager.AtkList.UltMegaFireworks;
                break;
            case (int)AIListManager.AtkList.WhirlFireCombo:
                bulletType = AIListManager.AtkList.WhirlFireCombo;
                break;
            case (int)AIListManager.AtkList.BoostFireCombo:
                bulletType = AIListManager.AtkList.BoostFireCombo;
                break;
            case (int)AIListManager.AtkList.WhirlBoostCombo:
                bulletType = AIListManager.AtkList.WhirlBoostCombo;
                break;
            case (int)AIListManager.AtkList.Ultimate:
                bulletType = AIListManager.AtkList.Ultimate;
                break;
        }

        // 生成予定データリストにこのデータを追加
        BulletSpawner.Instance.bulletDataList.Add(this);
    }
}
