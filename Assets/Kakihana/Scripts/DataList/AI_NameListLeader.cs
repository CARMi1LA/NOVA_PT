﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NameListLeader : MonoBehaviour
{
    //  中ボスタイプの敵AIリスト情報クラス

    public Dictionary<int, float> apprProbs;
    public Dictionary<int, float> waitProbs;
    public Dictionary<int, float> atkProbs;
    public Dictionary<int, float> escProbs;

    // 接近AIリスト
    public enum AI_Approach 
    {
         Normal = 0,                        // 最短距離でプレイヤーに接近
    };

    // 待機AIリスト
    public enum AI_Wait 
    {
        normal = 0,                         // その場で動かずに待機
        Quick = 2                           // その場で動かずに待機、次のAIに移行する時間がNormalより短い
    };

    // 攻撃AIリスト
    public enum AI_Attack 
    {
         Scatter = 1,                  // 多方向に向けて弾を撃つ
         Fireworks = 2,                // 全方向に向けて弾を撃つ
         Booster = 3,                  // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
         Bomb = 4,                     // 特定の広範囲に攻撃
         Bound = 5,                    // バウンドする弾を発射し、一定時間後にその地点からFireworksと同じ弾を出す
         LightRay = 8,                 // ビームによる攻撃を行う
         Whirlpool = 9,                // 渦巻状に弾を撃つ
         Forrow = 10,                  // プレイヤーを追従する弾を撃つ        // 中ボスクラス専用攻撃
         WhirlScatterCombo = 11,       // Whirlpool&Scatterの同時攻撃
         FireworksCombo = 12,          // 弾速が遅いFireworks発射後、角度を変えて弾速が早いFireworksを発射
         UltMegaFireworks = 13         // Fireworksを多数生成
    };

    // 逃走AIリスト
    public enum AI_Escape
    {
        Normal = 0,
    };

    // 敵AIレベルに応じてAIの強さを設定する
    public void EnemyAIProbSet(EnemyStatus.AI_Level ai_Level)
    {
        switch (ai_Level)
        {
            case EnemyStatus.AI_Level.Level1:
                AIProbInitLevel1();
                break;
            case EnemyStatus.AI_Level.Level2:
                AIProbInitLevel2();
                break;
            case EnemyStatus.AI_Level.Level3:
                AIProbInitLevel3();
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
        atkProbs = new Dictionary<int, float>();

        waitProbs.Add(0, 85.0f);
        waitProbs.Add(1, 15.0f);

        atkProbs.Add(0, 10.0f);
        atkProbs.Add(1, 30.0f);
        atkProbs.Add(2, 15.0f);
        atkProbs.Add(3, 10.0f);
        atkProbs.Add(4, 5.0f);
        atkProbs.Add(5, 10.0f);
        atkProbs.Add(6, 10.0f);
        atkProbs.Add(7, 10.0f);
    }

    void AIProbInitLevel2()
    {
        apprProbs = new Dictionary<int, float>();
        waitProbs = new Dictionary<int, float>();
        atkProbs = new Dictionary<int, float>();
        escProbs = new Dictionary<int, float>();

        apprProbs.Add(0, 50.0f);
        apprProbs.Add(1, 25.0f);
        apprProbs.Add(2, 25.0f);

        waitProbs.Add(0, 75.0f);
        waitProbs.Add(1, 25.0f);

        atkProbs.Add(0, 40.0f);
        atkProbs.Add(1, 35.0f);
        atkProbs.Add(2, 15.0f);
        atkProbs.Add(3, 8.0f);
        atkProbs.Add(4, 2.0f);

        escProbs.Add(0, 50.0f);
        escProbs.Add(1, 20.0f);
        escProbs.Add(2, 30.0f);
    }

    void AIProbInitLevel3()
    {
        apprProbs = new Dictionary<int, float>();
        waitProbs = new Dictionary<int, float>();
        atkProbs = new Dictionary<int, float>();
        escProbs = new Dictionary<int, float>();

        apprProbs.Add(0, 30.0f);
        apprProbs.Add(1, 30.0f);
        apprProbs.Add(2, 40.0f);

        waitProbs.Add(0, 50.0f);
        waitProbs.Add(1, 50.0f);

        atkProbs.Add(0, 30.0f);
        atkProbs.Add(1, 30.0f);
        atkProbs.Add(2, 20.0f);
        atkProbs.Add(3, 15.0f);
        atkProbs.Add(4, 3.0f);
        atkProbs.Add(5, 2.0f);

        escProbs.Add(0, 20.0f);
        escProbs.Add(1, 20.0f);
        escProbs.Add(2, 60.0f);
    }
}
