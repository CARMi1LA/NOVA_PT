using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NameListBoss : MonoBehaviour
{
    // ボスの敵AIリスト情報クラス
    public Dictionary<int, float> waitProbs;        // 待機AIの確率と行動パターンのインデックスを保存する変数

    // ボスのAIリスト、ボスは移動を行わない
    public string[] AI_Approach =
    {
        "Normal",                   
    };

    // ボスの待機AIリスト
    public string[] AI_Wait =
    {
        "Normal",                   // その場で動かずに待機
        "Quick"                     // その場で動かずに待機、次のAIに移行する時間がNormalより短い
    };

    // ボスの攻撃リスト
    public string[] AI_Attack =
    {
        "Whirlpool",                // 渦巻状に弾を撃つ
        "Fireworks",                // 全方向に向けて弾を撃つ
        "Booster",                  // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
        "Bomb",                     // 特定の広範囲に攻撃
        "Bound",                    // バウンドする弾を発射し、一定時間後にその地点からFireworksと同じ弾を出す
        "Forrow",                   // プレイヤーを追従する弾を撃つ
        "LightRay",                 // ビームによる攻撃を行う
        // ここからボス専用攻撃 一定体力低下時にのみ実行 //
        "WhirlFireCombo",           // WhirlpoolとFireworksの同時攻撃
        "BoostBoundRayCombo",       // BoosterとBoundとLightRayの同時攻撃
        "WhirlBoostCombo",          // WhirlpoolとBoosterの同時攻撃
        "Ultimate"                  // DPSチェックによる攻撃、弾を4発召喚し、一定時間内にダメージを与えないと大爆発
    };

    // 逃走AIリスト
    public string[] AI_Escape =
    {
        "Normal",
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
