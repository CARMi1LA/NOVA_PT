using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AIListManager : MonoBehaviour
{
    // AIのステート名称を格納しているクラス

    // 全接近AIリスト
    public enum ApprList
    {
         Normal = 0,               // 最短距離でプレイヤーに接近
         Wave,                     // ジグザグしながらプレイヤーに接近
         HighSpeed,                // 最短距離でプレイヤーに高速で接近
         LowSpeed,                 // 最短距離でプレイヤーにゆっくり接近
         EnemyGuard                // 最短距離で「近くの防御タイプ以外の敵」に接近
    }

    // 全待機AIリスト
    public enum WaitList
    {
         Normal = 0,               // 次の行動までその場で待機
         Follow,                   // 次の行動まで左右にゆっくり移動しながら待機
         Quick                      // その場で動かずに待機、次のAIに移行する時間がNormalより短い
    }

    // 全攻撃AIリスト
    public enum AtkList
    {
         Normal = 0,                   // 通常の弾を撃つ
         Scatter,                      // 多方向に向けて弾を撃つ
         Fireworks,                    // 全方向に向けて弾を撃つ
         Booster,                      // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
         Bomb,                         // 特定の広範囲に攻撃
         Bound,                        // バウンドする弾を発射し、一定時間後にその地点からFireworksと同じ弾を出す
         None,                         // 攻撃しない
         Bush,                         // 突進攻撃を行う
         LightRay,                     // ビームによる攻撃を行う
                 // 中ボスクラス専用攻撃
         WhirlScatterCombo,            // Whirlpool&Scatterの同時攻撃
         FireworksCombo,               // 弾速が遅いFireworks発射後、角度を変えて弾速が早いFireworksを発射
         UltMegaFireworks,             // Fireworksを多数生成
                 // ここからボス専用攻撃 一定体力低下時にのみ実行 //
         WhirlFireCombo,               // WhirlpoolとFireworksの同時攻撃
         BoostBoundRayCombo,           // BoosterとBoundとLightRayの同時攻撃
         WhirlBoostCombo,              // WhirlpoolとBoosterの同時攻撃
         Ultimate                      // DPSチェックによる攻撃、弾を4発召喚し、一定時間内にダメージを与えないと大爆発
    }
    // 全逃走AIリスト
    public enum EscList
    {
         Normal = 0,                   // プレイヤーと真逆の方向に逃走を試みる
         Wave,                         // ジグザグに進みながら逃走を試みる
         HighSpeed                     // プレイヤーと真逆の方向に高速で逃走を試みる
    }

    public ApprList apprName;
    public WaitList waitName;
    public AtkList atkName;
    public EscList escName;

    public AI_NameListAttack AI_AtkList;
    public AI_NameListDefence AI_DefList;
    public AI_NameListLeader AI_LeaderList;
    public AI_NameListBoss AI_BossList;
}