using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDEnemyManager : MonoBehaviour
{
    // 敵キャラクターの管理
     public TDEnemyData eData;

    // 親、サイズ、タイプの設定
    public TDList.ParentList eParent = TDList.ParentList.Enemy;
    public TDList.EnemySizeList eSize;
    public TDList.EnemyTypeList eType;

    // 標的の設定
    // プレイヤーを標的にするか
    public BoolReactiveProperty isTargetPlayer = new BoolReactiveProperty(false);
    public Transform targetTsf;              // 標的のタワーの位置
    public Transform playerTsf;              // プレイヤーの位置
    float TargetPlayerDistance = 200.0f;        // プレイヤーを追い続ける距離(離れすぎるとタワー狙いに戻る)

    public ParticleSystem towerHitParticle; // タワー特攻時のエフェクト
    public float towerHitInterval = 3.0f;          // タワー特攻時の待機時間

    public ParticleSystem deathParticle;

    // ユニットの初期化 <生成タイプ毎のエネミーデータ>
    public Subject<TDEnemyData>                     InitTrigger     = new Subject<TDEnemyData>();

    // AIによる操作イベント
    // 通常攻撃 <On/Off>
    public Subject<bool>                            AttackTrigger   = new Subject<bool>();

    /*
    // 移動 <入力データ>
    public Subject<Unit>                            MoveTrigger     = new Subject<Unit>();
    // スキル発動 <スキルの型>
    public Subject<TDPlayerData.SkillTypeList>      skillTrigger    = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動 <アルティメットの型>
    public Subject<TDPlayerData.UltimateTypeList>   ultimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();
    */

    // アクションイベント
    // タワーに当たったら…
    public Subject<TowerManager>                    TowerHitTrigger = new Subject<TowerManager>();
    // スロウトラップを踏んだ<bool>
    public BoolReactiveProperty                     SlowTrigger     = new BoolReactiveProperty(false);
    // 衝突 <衝突ObjのPosition>
    public Subject<Vector3>                         ImpactTrigger   = new Subject<Vector3>();
    // 死亡演出発動 <>
    public Subject<Unit>                            CoreDeathTrigger= new Subject<Unit>();

    void Start()
    {
        //playerPosition = GameManagement.Instance.playerTrans.position;
        isTargetPlayer.Value = true;

        // コアが破壊された場合の処理
        CoreDeathTrigger
            .Subscribe(_ =>
            {
                Destroy(this.gameObject);

            }).AddTo(this.gameObject);

        // プレイヤーを標的にし続けるか否かの判断をする
        this.UpdateAsObservable()
            .Where(x => eData.eSize != TDList.EnemySizeList.Extra)
            .Where(x => eData.eSize != TDList.EnemySizeList.Large)
            .Where(x => isTargetPlayer.Value)
            .Subscribe(_ =>
            {
                // プレイヤーとの距離を計算し、離れすぎた場合にターゲットをタワーに戻す
                float dis = (this.transform.position - playerTsf.position).sqrMagnitude;
                if(dis >= Mathf.Pow(TargetPlayerDistance, 2))
                {
                    isTargetPlayer.Value = false;
                }

            }).AddTo(this.gameObject);
    }
    // スポナーからのエネミーデータの受け取りおよび反映
    public void InitEnemyData(TDEnemyData data)
    {
        eData = data;
        InitTrigger.OnNext(eData);
    }
}
