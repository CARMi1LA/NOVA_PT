using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NameListDefence : MonoBehaviour
{
    // 防御タイプの敵AIリスト情報クラス

    public Dictionary<int, float> apprProbs;        // 接近AIの確率と行動パターンのインデックスを保存する変数
    public Dictionary<int, float> waitProbs;        // 待機AIの確率と行動パターンのインデックスを保存する変数
    public Dictionary<int, float> atkProbs;         // 攻撃AIの確率と行動パターンのインデックスを保存する変数
    public Dictionary<int, float> escProbs;         // 逃走AIの確率と行動パターンのインデックスを保存する変数

    // 接近AIリスト
    public enum AI_Approach 
    {
        Normal = 0,                        // 最短距離でプレイヤーに接近
        LowSpeed = 3,                      // 最短距離でプレイヤーにゆっくり接近
        EnemyGuard = 4                     // 最短距離で「近くの防御タイプ以外の敵」
    };

    // 待機AIリスト
    public enum AI_Wait 
    {
        Normal = 0,                        // 次の行動までその場で待機
    };

    public enum AI_Attack 
    {
         None = 6,                         // 攻撃しない
         Bush = 7,                         // 突進攻撃を行う
         LightRay = 8,                     // ビームによる攻撃を行う
    };

    public enum AI_Escape 
    {
         Normal = 0,                       // プレイヤーと真逆の方向に逃走を試みる
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

        apprProbs.Add(0, 20.0f);
        apprProbs.Add(1, 80.0f);

        waitProbs.Add(0, 100.0f);

        atkProbs.Add(0, 90.0f);
        atkProbs.Add(1, 10.0f);

        escProbs.Add(0, 100.0f);
    }

    void AIProbInitLevel2()
    {
        apprProbs = new Dictionary<int, float>();
        waitProbs = new Dictionary<int, float>();
        atkProbs = new Dictionary<int, float>();
        escProbs = new Dictionary<int, float>();

        apprProbs.Add(0, 30.0f);
        apprProbs.Add(1, 60.0f);
        apprProbs.Add(2, 10.0f);

        waitProbs.Add(0, 100.0f);

        atkProbs.Add(0, 60.0f);
        atkProbs.Add(1, 35.0f);
        atkProbs.Add(2, 5.0f);

        escProbs.Add(0, 100.0f);
    }

    void AIProbInitLevel3()
    {
        apprProbs = new Dictionary<int, float>();
        waitProbs = new Dictionary<int, float>();
        atkProbs = new Dictionary<int, float>();
        escProbs = new Dictionary<int, float>();

        apprProbs.Add(0, 40.0f);
        apprProbs.Add(1, 20.0f);
        apprProbs.Add(2, 40.0f);

        waitProbs.Add(0, 100.0f);

        atkProbs.Add(0, 20.0f);
        atkProbs.Add(1, 50.0f);
        atkProbs.Add(2, 30.0f);

        escProbs.Add(0, 100.0f);
    }
}
