using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDEnemyManager : MonoBehaviour
{


    // プレイヤーを発見しているか
    bool isLookPlayer = false;

    // ボタンイベント
    // 移動 <入力データ>
    public Subject<Unit>                            moveTrigger     = new Subject<Unit>();
    // ダッシュ発動 <>
    public Subject<Unit>                            dashTrigger     = new Subject<Unit>();
    // 通常攻撃 <On/Off>
    public Subject<bool>                            attackTrigger   = new Subject<bool>();
    // スキル発動 <スキルの型>
    public Subject<TDPlayerData.SkillTypeList>      skillTrigger    = new Subject<TDPlayerData.SkillTypeList>();
    // アルティメット発動 <アルティメットの型>
    public Subject<TDPlayerData.UltimateTypeList>   ultimateTrigger = new Subject<TDPlayerData.UltimateTypeList>();

    // アクションイベント
    // 衝突 <衝突ObjのPosition>
    public Subject<Vector3>                         impactTrigger   = new Subject<Vector3>();
    // ダメージ演出発動 <>
    public Subject<Unit>                            DamageTrigger   = new Subject<Unit>();
    // 死亡演出発動 <>
    public Subject<Unit>                            DeathTrigger    = new Subject<Unit>();
    // 回復イベント
    public Subject<Unit>                            HealTrigger     = new Subject<Unit>();

    void Start()
    {
        
    }
}
