using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TowerManager : MonoBehaviour,ITwDamage
{
    // タワー（拠点）スクリプト

    // 拠点の色、タワー識別用
    public ShopData.TowerColor towerColor;
    // タワーのレベル
    private LevelData_Tower towerLv;
    // 敵情報リスト
    private EnemyInfoList enemyList;

    // 最大HP
    [SerializeField] private int tower_MaxHP = 100;
    // 防衛認識距離
    [SerializeField] private int tower_RecogDis;
    // 現在の標的
    [SerializeField] Transform targetEnemy;
    // 現在のHP
    [SerializeField] private IntReactiveProperty towerHp = new IntReactiveProperty(0);
    // 自タワーのトラップ情報
    [SerializeField] private TrapManager trap;
    // 自タワーのタレット情報
    [SerializeField] private TurretManager[] turrets;

    // タワーバリア使用可能フラグ
    public BoolReactiveProperty barrierFlg = new BoolReactiveProperty(true);
    // 現在タワーが生存しているか
    public BoolReactiveProperty towerDeath = new BoolReactiveProperty(false);

    public Subject<int> trapUpdate = new Subject<int>();

    // Start is called before the first frame update
    void Start()
    {
        // タワー情報をショップより取得
        towerLv = ShopManager.Instance.shopData.levelData_Tower[(int)towerColor];
        // 現在HPの設定
        towerHp.Value = tower_MaxHP;

        towerLv.level_Trap.Subscribe(_ =>
            {
                trap.trapSpeed = trap.trapSpeedData[towerLv.level_Trap.Value];
            }).AddTo(this.gameObject);

        towerLv.level_Turret.Subscribe(_ =>
            {
                turrets[towerLv.level_Turret.Value].turretActive.Value = true;
            }).AddTo(this.gameObject);

        towerLv.level_Tower.Subscribe(_ =>
            {

            }).AddTo(this.gameObject);

        // ショップで修理ボタンが押された時の処理
        towerLv.level_Repair.Subscribe(_ =>
            {
                // 最大HPの25％ぶん回復、但し最大HP以上は回復しない
                towerHp.Value += Mathf.RoundToInt(tower_MaxHP * 0.25f);
                if (towerHp.Value >= tower_MaxHP)
                {
                    towerHp.Value = tower_MaxHP;
                }
            }).AddTo(this.gameObject);

        // １秒毎に敵情報リストと自タワーとの距離を測り、接近していればバリア起動
        this.UpdateAsObservable()
            .Sample(System.TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => 
            {
                var dis = 0.0f;
                foreach (var item in enemyList.enemyInfo)
                {
                    dis = (this.transform.position - item.position).sqrMagnitude;
                    if (dis <= Mathf.Pow(tower_RecogDis,2) && barrierFlg.Value == true)
                    {
                        barrierFlg.Value = false;
                    }
                }
            }).AddTo(this.gameObject);

        // バリア復活処理１０秒経過で使用可能
        barrierFlg.Where(_ => barrierFlg.Value == false)
            .Sample(System.TimeSpan.FromSeconds(10.0f))
            .Subscribe(_ => 
            {
                barrierFlg.Value = true;
            }).AddTo(this.gameObject);


    }
     void ITwDamage.HitDamage(int atk)
    {
        towerHp.Value -= atk;
    }
}