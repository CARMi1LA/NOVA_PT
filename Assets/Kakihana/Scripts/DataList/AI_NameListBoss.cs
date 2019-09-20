using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NameListBoss : MonoBehaviour
{
    // ボスの敵AIリスト情報クラス
    public Dictionary<int, float> waitProbs;        // 待機AIの確率と行動パターンのインデックスを保存する変数

    // ボスのAIリスト、ボスは移動を行わない
    public enum AI_Approach 
    {
        Normal = 0,                   
    };

    // ボスの待機AIリスト
    public enum AI_Wait 
    {
         normal = 0,                // その場で動かずに待機
         Quick = 2                  // その場で動かずに待機、次のAIに移行する時間がNormalより短い
    };

    // ボスの攻撃リスト
    public enum AI_Attack 
    {
         Fireworks = 2,                // 全方向に向けて弾を撃つ
         Booster = 3,                  // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
         Bound = 5,                    // バウンドする弾を発射し、一定時間後にその地点からFireworksと同じ弾を出す
         LightRay = 8,                 // ビームによる攻撃を行う
         Whirlpool = 9,                // 渦巻状に弾を撃つ
         Forrow = 10,                  // プレイヤーを追従する弾を撃つ
         // ここからボス専用攻撃 一定体力低下時にのみ実行 //
         WhirlFireCombo = 21,          // WhirlpoolとFireworksの同時攻撃
         BoostBoundRayCombo = 22,      // BoosterとBoundとLightRayの同時攻撃
         WhirlBoostCombo = 23,         // WhirlpoolとBoosterの同時攻撃
         Ultimate = 24                 // DPSチェックによる攻撃、弾を4発召喚し、一定時間内にダメージを与えないと大爆発
    };

    // 逃走AIリスト
    public enum AI_Escape 
    {
        Normal = 0,
    };

    public void EnemyAIProbSet(EnemyStatus.AI_Level ai_Level)
    {
        switch (ai_Level)
        {
            case EnemyStatus.AI_Level.Level1:
                AIProbInitLevel1();
                break;
            default:
                break;
        }
    }

    // Level1の行動パターン確率の割り振り
    // Addの第一引数はAIリストのインデックス(Int型）、第二引数は確率（Float型）
    void AIProbInitLevel1()
    {
        waitProbs = new Dictionary<int, float>();

        waitProbs.Add(0, 70.0f);
        waitProbs.Add(1, 30.0f);
    }
}
