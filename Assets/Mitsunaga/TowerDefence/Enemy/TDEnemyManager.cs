using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDEnemyManager : MonoBehaviour
{
    // 敵キャラクターの管理
    TDEnemyData eData;

    public TDList.ParentList eParent = TDList.ParentList.Enemy;

    // プレイヤーを発見しているか
    [SerializeField]
    bool isLookPlayer = false;
    [SerializeField]
    GameObject PlayerObject;

    // ユニットの初期化 <生成タイプ毎のエネミーデータ>
    public Subject<TDEnemyData>                     InitTrigger     = new Subject<TDEnemyData>();


    // AIによる操作イベント
    // 移動 <入力データ>
    public Subject<Unit>                            MoveTrigger     = new Subject<Unit>();
    // 通常攻撃 <On/Off>
    public Subject<bool>                            AttackTrigger   = new Subject<bool>();
    /*
    // スキル発動 <スキルの型>
    public Subject<TDPlayerData.SkillTypeList>      skillTrigger    = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動 <アルティメットの型>
    public Subject<TDPlayerData.UltimateTypeList>   ultimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();
    */

    // アクションイベント
    // 衝突 <衝突ObjのPosition>
    public Subject<Vector3>                         ImpactTrigger   = new Subject<Vector3>();
    // 死亡演出発動 <>
    public Subject<Unit>                            CoreDeathTrigger    = new Subject<Unit>();

    void Start()
    {
        initEnemy(0, 0);

        CoreDeathTrigger
            .Subscribe(_ =>
            {
                Debug.Log("こあしんだ");
                Destroy(this.gameObject);

            }).AddTo(this.gameObject);
    }

    // これスポナーにやらせよう
    public void initEnemy(TDList.EnemySizeList size,TDList.EnemyTypeList type)
    {
        // リソースからエネミーのサイズとタイプに応じた情報を取得、それぞれを初期化する
        eData = Resources.Load<TDEnemyDataList>("TDEnemyDataList").GetEnemyData(size, type);
        Debug.Log(eData.ToString());
        InitTrigger.OnNext(eData);
    }
}
