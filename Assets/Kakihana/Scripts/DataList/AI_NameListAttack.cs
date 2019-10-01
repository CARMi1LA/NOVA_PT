﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_NameListAttack : MonoBehaviour
{
    // 攻撃タイプの敵AIリスト情報クラス

    public Dictionary<int, float> apprProbs;
    public Dictionary<int, float> waitProbs;
    public Dictionary<int, float> atkProbs;
    public Dictionary<int, float> escProbs;

    // 接近AIリスト
    // 接近AIリスト
    public enum AI_Approach
    {
        Normal = 0,                        // 最短距離でプレイヤーに接近
        Wave = 1,                          // 最短距離でプレイヤーにゆっくり接近
        HighSpeed = 2                     // 最短距離で「近くの防御タイプ以外の敵」
    };

    // 待機AIリスト
    public enum AI_Wait 
    {
         Normal = 0,                        // 次の行動までその場で待機
    };

    // 攻撃AIリスト
    public enum AI_Attack 
    {
        Normal = 0,                        // 通常の弾を撃つ
        Scatter = 1,                       // 多方向に向けて弾を撃つ
        Fireworks = 2,                     // 全方向に向けて弾を撃つ
        Booster = 3,                       // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
        Bomb = 4,                          // 特定の広範囲に攻撃
        Bound = 5,                         // バウンドする弾を発射し、一定時間後にその地点からFireworksと同じ弾を出す
    };

    // 逃走AIリスト
    public enum AI_Escape 
    {
        Normal = 0,                         // プレイヤーと真逆の方向に逃走を試みる
        HighSpeed = 1                       // プレイヤーと真逆の方向に高速で逃走を試みる
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
        apprProbs = new Dictionary<int, float>();
        waitProbs = new Dictionary<int, float>();
        atkProbs = new Dictionary<int, float>();
        escProbs = new Dictionary<int, float>();

        apprProbs.Add(0, 90.0f);
        apprProbs.Add(1, 5.0f);
        apprProbs.Add(2, 5.0f);

        waitProbs.Add(0, 100.0f);

        atkProbs.Add(0, 55.0f);
        atkProbs.Add(1, 35.0f);
        atkProbs.Add(2, 10.0f);

        escProbs.Add(0, 90.0f);
        escProbs.Add(1, 10.0f);
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

        waitProbs.Add(0, 100.0f);

        atkProbs.Add(0, 40.0f);
        atkProbs.Add(1, 35.0f);
        atkProbs.Add(2, 15.0f);
        atkProbs.Add(3, 8.0f);
        atkProbs.Add(4, 2.0f);

        escProbs.Add(0, 80.0f);
        escProbs.Add(1, 20.0f);
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

        waitProbs.Add(0, 100.0f);

        atkProbs.Add(0, 30.0f);
        atkProbs.Add(1, 30.0f);
        atkProbs.Add(2, 20.0f);
        atkProbs.Add(3, 15.0f);
        atkProbs.Add(4, 3.0f);
        atkProbs.Add(5, 2.0f);

        escProbs.Add(0, 50.0f);
        escProbs.Add(1, 50.0f);
    }
}
