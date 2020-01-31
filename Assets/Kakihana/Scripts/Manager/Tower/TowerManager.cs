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
    public LevelData_Tower towerLv;
    // 敵情報リスト
    private EnemyInfoList enemyList;

    // 最大HP
    [SerializeField] private int tower_MaxHP = 100;
    // タワーバフ支援距離
    [SerializeField] private int tower_RecogDis;
    // 自動回復間隔
    public FloatReactiveProperty autoRepairInterval = new FloatReactiveProperty(0);
    // 自動回復量
    public int autoRepairValue;
    // 自動回復間隔リスト
    public int[] autoRepairValueList;
    // 現在の標的
    [SerializeField] Transform targetEnemy;
    // 敵スポーン位置
    public Transform spawnPos;
    // 現在のHP
    public IntReactiveProperty towerHp = new IntReactiveProperty(0);
    // 自タワーのトラップ情報
    [SerializeField] private TrapManager trap;
    // 自タワーのタレット情報
    [SerializeField] private TurretManager[] turrets;

    // タワーバリア使用可能フラグ
    public BoolReactiveProperty barrierFlg = new BoolReactiveProperty(true);
    // 現在タワーが生存しているか
    public BoolReactiveProperty towerDeath = new BoolReactiveProperty(false);
    // 究極タワー状態フラグ
    public BoolReactiveProperty towerUlt = new BoolReactiveProperty(false);

    // タワー自動回復Subject
    public Subject<Unit> autoRepair = new Subject<Unit>();


    void Awake()
    {
        // 現在HPの設定
        towerHp.Value = tower_MaxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        // タワー情報をショップより取得
        towerLv = ShopManager.Instance.spLv.towerLv[(int)towerColor];
        // タワー自動回復間隔の設定
        autoRepairInterval.Value = 1.0f;

        // タワー自動回復
        autoRepair
        .Subscribe(_ => 
        {
            towerHp.Value += autoRepairValue;
        }).AddTo(this.gameObject);

        // タレット初期設定
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].turretIntSet.OnNext(towerLv.level_Tower.Value);
            turrets[i].gameObject.SetActive(false);
        }

        // トラップ購入ボタンが押された時の処理
        towerLv.level_Trap.Subscribe(_ =>
            {
                // 各レベルの情報を適用する
                trap.trapSizeZ = trap.trapSizeListZ[towerLv.level_Trap.Value];
                trap.lvUpSub.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // タレット購入ボタンが押された時の処理
        towerLv.level_Turret
            .Where(_ => towerLv.level_Turret.Value >= 1)
            .Subscribe(_ =>
            {
                // タレット表示
                turrets[towerLv.level_Turret.Value - 1].gameObject.SetActive(true);
                // タレットをアクティブ状態に
                turrets[towerLv.level_Turret.Value - 1].turretActive.Value = true;
                // タレット攻撃間隔の設定
                turrets[towerLv.level_Turret.Value - 1].turretIntSet.OnNext(towerLv.level_Tower.Value);
            }).AddTo(this.gameObject);

        // タワー購入ボタンが押された時の処理
        towerLv.level_Tower.Subscribe(_ =>
            {
                // 全タレットの攻撃間隔更新
                for (int i = 0; i < turrets.Length; i++)
                {
                    turrets[i].turretIntSet.OnNext(towerLv.level_Tower.Value);
                }
                // トラップの横幅サイズの設定
                trap.trapSizeX = trap.trapSizeListX[towerLv.level_Tower.Value];
                // トラップサイズの更新
                trap.lvUpSub.OnNext(Unit.Default);
                // タワー自動回復量の設定
                autoRepairValue = autoRepairValueList[towerLv.level_Tower.Value];
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

        // HPが0になるとタワー消滅
        towerHp.Where(_ => towerHp.Value <= 0 && towerDeath.Value == false)
            .Subscribe(_ => 
            {
                towerDeath.Value = true;
                GameManagement.Instance.towerDeathSub.OnNext(Unit.Default);
            }).AddTo(this.gameObject);

        // タワー支援条件式
        this.UpdateAsObservable()
            .Where(_ => towerLv.level_Tower.Value == towerLv.MAX_LV)
            .Where(_ => towerDeath.Value == false)
            .Subscribe(_ => 
            {
                // タワーシステムレベルがMAXかつそのタワーの付近にいると支援発動
                var dis = 0.0f;
                dis = (this.transform.position - GameManagement.Instance.playerTrans.position).sqrMagnitude;
                if (dis <= Mathf.Pow(tower_RecogDis,2))
                {
                    GameManagement.Instance.towerUltOn.OnNext(towerColor);
                }
                else
                {
                    GameManagement.Instance.towerUltOff.OnNext(towerColor);
                }
            }).AddTo(this.gameObject);

        // 赤タワー支援処理（支援内容：タレットの発射間隔を更に減少）
        GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Red]
            .Where(_ => towerDeath.Value == false)
            .Subscribe(_ => 
            {
                if (GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Red].Value == true)
                {
                    for (int i = 0; i < turrets.Length; i++)
                    {
                        // タレット攻撃間隔の設定
                        turrets[i].turretShotInterval = turrets[i].turretShotInterval * 0.5f;
                    }
                }
                else
                {
                    for (int i = 0; i < turrets.Length; i++)
                    {
                        // タレット攻撃間隔の設定
                        turrets[i].turretIntSet.OnNext(towerLv.level_Tower.Value);
                    }
                }
            }).AddTo(this.gameObject);

        // 青タワー支援内容（支援内容：スロートラップの効果範囲増加）
        GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Blue]
            .Where(_ => towerDeath.Value == false)
            .Subscribe(_ => 
            {
                if (GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Blue].Value == true)
                {
                    trap.trapSizeX = trap.trapSizeX * 1.5f;
                    trap.trapSizeZ = trap.trapSizeZ * 1.5f;
                    // トラップサイズの更新
                    trap.lvUpSub.OnNext(Unit.Default);
                }
                else
                {
                    // 各レベルの情報を適用する
                    trap.trapSizeZ = trap.trapSizeListZ[towerLv.level_Trap.Value];
                    // トラップの横幅サイズの設定
                    trap.trapSizeX = trap.trapSizeListX[towerLv.level_Tower.Value];
                    // トラップサイズの更新
                    trap.lvUpSub.OnNext(Unit.Default);
                }
            }).AddTo(this.gameObject);

        // 黄タワー支援処理（支援内容：タワー生存数×25マター獲得）
        this.UpdateAsObservable()
            .Where(_ => towerDeath.Value == false)
            .Where(_ => GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Yellow].Value == true)
            .Sample(System.TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                GameManagement.Instance.addMater.OnNext(25);
            }).AddTo(this.gameObject);

        // 緑タワー支援内容（支援内容：1秒あたりのタワー回復量更に増加）
        GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Green]
            .Where(_ => towerDeath.Value == false)
            .Subscribe(_ =>
            {
                if (GameManagement.Instance.towerUltFlg[(int)MasterData.TowerColor.Green].Value == true)
                {
                    autoRepairValue = 3;
                }
                else
                {
                    // タワー自動回復間隔の設定
                    autoRepairValue = autoRepairValueList[towerLv.level_Tower.Value];
                }
            }).AddTo(this.gameObject);

        // タワーが生存していてHPが満タンでなければ自動回復する
        this.UpdateAsObservable()
            .Where(_ => towerDeath.Value == false)
            .Where(_ => towerHp.Value != tower_MaxHP)
            .Sample(System.TimeSpan.FromSeconds(autoRepairInterval.Value))
            .Subscribe(_ =>
            {
                autoRepair.OnNext(Unit.Default);
            }).AddTo(this.gameObject);
    }

    // タワーダメージインターフェース
    public void HitDamage(int atk)
    {
        towerHp.Value -= atk;
    }
}