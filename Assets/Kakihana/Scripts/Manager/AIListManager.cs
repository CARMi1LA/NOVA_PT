using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AIListManager : MonoBehaviour
{
    // AIのステート名称を格納しているクラス
    // 行動パターン名とIDで関連付けを行う

    // 全接近AIリスト
    public enum ApprList
    {
         Normal = 0,                        // 最短距離でプレイヤーに接近
         Wave = 1,                          // ジグザグしながらプレイヤーに接近
         HighSpeed = 2,                     // 最短距離でプレイヤーに高速で接近
         LowSpeed = 3,                      // 最短距離でプレイヤーにゆっくり接近
         EnemyGuard = 4                     // 最短距離で「近くの防御タイプ以外の敵」に接近
    }

    // 全待機AIリスト
    public enum WaitList
    {
         Normal = 0,                        // 次の行動までその場で待機
         Follow = 1,                        // 次の行動まで左右にゆっくり移動しながら待機
         Quick = 2                          // その場で動かずに待機、次のAIに移行する時間がNormalより短い
    }

    // 全攻撃AIリスト
    public enum AtkList
    {
         Normal = 0,                        // 通常の弾を撃つ
         Scatter = 1,                       // 多方向に向けて弾を撃つ
         Fireworks = 2,                     // 全方向に向けて弾を撃つ
         Booster = 3,                       // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
         Bomb = 4,                          // 特定の広範囲に攻撃
         Bound = 5,                         // バウンドする弾を発射し、一定時間後にその地点からFireworksと同じ弾を出す
         None = 6,                          // 攻撃しない
         Bush = 7,                          // 突進攻撃を行う
         LightRay = 8,                      // ビームによる攻撃を行う
         Whirlpool = 9,                     // 渦巻状に弾を発射
         Forrow = 10,                       // プレイヤーを追尾する弾を発射
         // 中ボスクラス専用攻撃
         WhirlScatterCombo = 11,            // Whirlpool&Scatterの同時攻撃
         FireworksCombo = 12,               // 弾速が遅いFireworks発射後、角度を変えて弾速が早いFireworksを発射
         UltMegaFireworks = 13,             // Fireworksを多数生成
         // ここからボス専用攻撃 一定体力低下時にのみ実行 //
         WhirlFireCombo = 21,               // WhirlpoolとFireworksの同時攻撃
         BoostBoundRayCombo = 22,           // BoosterとBoundとLightRayの同時攻撃
         WhirlBoostCombo = 23,              // WhirlpoolとBoosterの同時攻撃
         Ultimate = 24                      // DPSチェックによる攻撃、弾を4発召喚し、一定時間内にダメージを与えないと大爆発
    }
    // 全逃走AIリスト
    public enum EscList
    {
         Normal = 0,                         // プレイヤーと真逆の方向に逃走を試みる
         Wave = 1,                           // ジグザグに進みながら逃走を試みる
         HighSpeed = 2                       // プレイヤーと真逆の方向に高速で逃走を試みる
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