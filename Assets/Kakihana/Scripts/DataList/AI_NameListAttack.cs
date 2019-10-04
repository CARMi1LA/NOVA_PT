using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_NameListAttack : MonoBehaviour
{
    // 攻撃タイプの敵AIリスト情報クラス

    public Dictionary<AI_Approach, float> apprProbs;
    public Dictionary<AI_Wait, float> waitProbs;
    public Dictionary<AI_Attack, float> atkProbs;
    public Dictionary<AI_Escape, float> escProbs;

    // 接近AIリスト
    // 接近AIリスト
    public enum AI_Approach
    {
        Normal = 0,                        // 最短距離でプレイヤーに接近
        HighSpeed = 1                     // 最短距離で「近くの防御タイプ以外の敵」
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
    };

    // 逃走AIリスト
    public enum AI_Escape 
    {
        Normal = 0,                         // プレイヤーと真逆の方向に逃走を試みる
        HighSpeed = 1                       // プレイヤーと真逆の方向に高速で逃走を試みる
    };

    public void EnemyAIProbSetAppr(EnemyStatus.AI_Level ai_Level)
    {
        switch (ai_Level)
        {
            case EnemyStatus.AI_Level.Level1:
                apprProbs = new Dictionary<AI_Approach, float>();

                apprProbs.Add(AI_Approach.Normal, 90.0f);
                apprProbs.Add(AI_Approach.HighSpeed, 10.0f);
                break;
            case EnemyStatus.AI_Level.Level2:
                apprProbs = new Dictionary<AI_Approach, float>();

                apprProbs.Add(AI_Approach.Normal, 70.0f);
                apprProbs.Add(AI_Approach.HighSpeed, 30.0f);
                break;
            case EnemyStatus.AI_Level.Level3:
                apprProbs = new Dictionary<AI_Approach, float>();
                apprProbs.Add(AI_Approach.Normal, 50.0f);
                apprProbs.Add(AI_Approach.HighSpeed, 50.0f);
                break;
            default:
                break;
        }
    }

    public void EnemyAIProbSetWait(EnemyStatus.AI_Level ai_Level)
    {
        switch (ai_Level)
        {
            case EnemyStatus.AI_Level.Level1:
                waitProbs = new Dictionary<AI_Wait, float>();
                waitProbs.Add(AI_Wait.Normal, 100.0f);
                break;
            case EnemyStatus.AI_Level.Level2:
                waitProbs = new Dictionary<AI_Wait, float>();
                waitProbs.Add(AI_Wait.Normal, 100.0f);
                break;
            case EnemyStatus.AI_Level.Level3:
                waitProbs = new Dictionary<AI_Wait, float>();
                waitProbs.Add(AI_Wait.Normal, 100.0f);
                break;
        }
    }

    public void EnemyAIProbSetAtk(EnemyStatus.AI_Level ai_Level)
    {
        switch (ai_Level)
        {
            case EnemyStatus.AI_Level.Level1:
                atkProbs = new Dictionary<AI_Attack, float>();
                atkProbs.Add(AI_Attack.Normal, 70.0f);
                atkProbs.Add(AI_Attack.Scatter, 20.0f);
                break;
            case EnemyStatus.AI_Level.Level2:
                atkProbs = new Dictionary<AI_Attack, float>();
                atkProbs.Add(AI_Attack.Normal, 60.0f);
                atkProbs.Add(AI_Attack.Scatter, 30.0f);
                atkProbs.Add(AI_Attack.Fireworks, 10.0f);
                break;
            case EnemyStatus.AI_Level.Level3:
                atkProbs = new Dictionary<AI_Attack, float>();
                atkProbs.Add(AI_Attack.Normal, 50.0f);
                atkProbs.Add(AI_Attack.Scatter, 30.0f);
                atkProbs.Add(AI_Attack.Fireworks, 15.0f);
                atkProbs.Add(AI_Attack.Booster, 5.0f);
                break;
        }
    }
    public void EnemyAIProbSetEsc(EnemyStatus.AI_Level ai_Level)
    {
        switch (ai_Level)
        {
            case EnemyStatus.AI_Level.Level1:
                escProbs = new Dictionary<AI_Escape, float>();
                escProbs.Add(AI_Escape.Normal, 100.0f);
                break;
            case EnemyStatus.AI_Level.Level2:
                escProbs = new Dictionary<AI_Escape, float>();
                escProbs.Add(AI_Escape.Normal, 80.0f);
                escProbs.Add(AI_Escape.Normal, 20.0f);
                break;
            case EnemyStatus.AI_Level.Level3:
                escProbs = new Dictionary<AI_Escape, float>();
                escProbs.Add(AI_Escape.Normal, 60.0f);
                escProbs.Add(AI_Escape.Normal, 40.0f);
                break;
        }
    }
}


    // 敵AIレベルに応じてAIの強さを設定する
    //public void EnemyAIProbSet(EnemyStatus.AI_Level ai_Level)
    //{
    //    switch (ai_Level)
    //    {
    //        case EnemyStatus.AI_Level.Level1:
    //            apprProbs = new Dictionary<AI_Approach, float>();
    //            waitProbs = new Dictionary<AI_Wait, float>();
    //            atkProbs = new Dictionary<AI_Attack, float>();
    //            escProbs = new Dictionary<AI_Escape, float>();

    //            apprProbs.Add(AI_Approach.Normal, 90.0f);
    //            apprProbs.Add(AI_Approach.HighSpeed, 10.0f);

    //            waitProbs.Add(AI_Wait.Normal, 100.0f);

    //            atkProbs.Add(AI_Attack.Normal, 70.0f);
    //            atkProbs.Add(AI_Attack.Scatter, 20.0f);

    //            escProbs.Add(AI_Escape.Normal, 100.0f); ;
    //            break;
    //        case EnemyStatus.AI_Level.Level2:
    //            AIProbInitLevel2();
    //            break;
    //        case EnemyStatus.AI_Level.Level3:
    //            AIProbInitLevel3();
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //// Level1の行動パターン確率の割り振り
    //// Addの第一引数はAIリスト）、第二引数は確率（Float型）
    //void AIProbInitLevel1()
    //{
    //    apprProbs = new Dictionary<AI_Approach, float>();
    //    waitProbs = new Dictionary<AI_Wait, float>();
    //    atkProbs = new Dictionary<AI_Attack, float>();
    //    escProbs = new Dictionary<AI_Escape, float>();

    //    apprProbs.Add(AI_Approach.Normal, 90.0f);
    //    apprProbs.Add(AI_Approach.HighSpeed, 10.0f);

    //    waitProbs.Add(AI_Wait.Normal, 100.0f);

    //    atkProbs.Add(AI_Attack.Normal, 70.0f);
    //    atkProbs.Add(AI_Attack.Scatter, 20.0f);

    //    escProbs.Add(AI_Escape.Normal, 100.0f);
    //}
    //// Level2の行動パターン確率の割り振り
    //void AIProbInitLevel2()
    //{
    //    apprProbs = new Dictionary<AI_Approach, float>();
    //    waitProbs = new Dictionary<AI_Wait, float>();
    //    atkProbs = new Dictionary<AI_Attack, float>();
    //    escProbs = new Dictionary<AI_Escape, float>();

    //    apprProbs.Add(AI_Approach.Normal, 70.0f);
    //    apprProbs.Add(AI_Approach.HighSpeed, 30.0f);

    //    waitProbs.Add(AI_Wait.Normal, 100.0f);

    //    atkProbs.Add(AI_Attack.Normal, 60.0f);
    //    atkProbs.Add(AI_Attack.Scatter, 30.0f);
    //    atkProbs.Add(AI_Attack.Fireworks, 10.0f);

    //    escProbs.Add(AI_Escape.Normal, 100.0f);
    //}
    //// Level3の行動パターン確率の割り振り
    //void AIProbInitLevel3()
    //{
    //    apprProbs = new Dictionary<AI_Approach, float>();
    //    waitProbs = new Dictionary<AI_Wait, float>();
    //    atkProbs = new Dictionary<AI_Attack, float>();
    //    escProbs = new Dictionary<AI_Escape, float>();

    //    apprProbs.Add(AI_Approach.Normal, 50.0f);
    //    apprProbs.Add(AI_Approach.HighSpeed, 50.0f);

    //    waitProbs.Add(AI_Wait.Normal, 100.0f);

    //    atkProbs.Add(AI_Attack.Normal, 50.0f);
    //    atkProbs.Add(AI_Attack.Scatter, 30.0f);
    //    atkProbs.Add(AI_Attack.Fireworks, 15.0f);
    //    atkProbs.Add(AI_Attack.Booster, 5.0f);

    //    escProbs.Add(AI_Escape.Normal, 100.0f);
    //}
