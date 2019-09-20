using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ActManager : MonoBehaviour
{
    // AIステートをもとに敵の移動量などを計算するクラス

    [SerializeField] private Transform playerTrans;         // プレイヤーの座標

    private void Awake()
    {
        // プレイヤーの座標を取得
        playerTrans = GameManagement.Instance.playerTrans;
    }

    void Start()
    {
        
    }

    // 接近モード処理
    public Vector3 CalcApprMove(Vector3 move, float speed,int actID)
    {
        // 行動IDに基づき、各処理を行う
        switch (actID)
        {
            case (int)AIListManager.ApprList.Normal:
                Vector3 dif = playerTrans.position - move;
                float radian = Mathf.Atan2(dif.z, dif.x);
                return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * speed * 10;
            case (int)AIListManager.ApprList.Wave:
                 dif = playerTrans.position - move;
                 radian = Mathf.Atan2(dif.z, dif.x);
                return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * speed * 10;
            case (int)AIListManager.ApprList.HighSpeed:
                 dif = playerTrans.position - move;
                 radian = Mathf.Atan2(dif.z, dif.x);
                return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * (speed * 2) * 10;
            case (int)AIListManager.ApprList.LowSpeed:
                dif = playerTrans.position - move;
                radian = Mathf.Atan2(dif.z, dif.x);
                return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * (speed * 0.5f) * 10;
            case (int)AIListManager.ApprList.EnemyGuard:
                break;
        }
        return Vector3.zero;
    }

    // 待機モード処理
    public Vector3 ActWaitCalc(Vector3 move,float speed,int actID)
    {
        switch (actID)
        {
            case (int)AIListManager.WaitList.Normal:
                break;
            case (int)AIListManager.WaitList.Follow:
                break;
            case (int)AIListManager.WaitList.Quick:
                break;
        }
        return Vector3.zero;
    }

    // 攻撃モード処理
    public void EnemyAtkCalc(int actID)
    {
        switch (actID)
        {
            case (int)AIListManager.AtkList.Normal:
                break;
            case (int)AIListManager.AtkList.Scatter:
                break;
            case (int)AIListManager.AtkList.Fireworks:
                break;
            case (int)AIListManager.AtkList.Booster:
                break;
            case (int)AIListManager.AtkList.Bomb:
                break;
            case (int)AIListManager.AtkList.None:
                break;
            case (int)AIListManager.AtkList.Bush:
                break;
            case (int)AIListManager.AtkList.LightRay:
                break;
            case (int)AIListManager.AtkList.Whirlpool:
                break;
            case (int)AIListManager.AtkList.Forrow:
                break;
            case (int)AIListManager.AtkList.WhirlScatterCombo:
                break;
            case (int)AIListManager.AtkList.FireworksCombo:
                break;
            case (int)AIListManager.AtkList.UltMegaFireworks:
                break;
            case (int)AIListManager.AtkList.WhirlFireCombo:
                break;
            case (int)AIListManager.AtkList.BoostBoundRayCombo:
                break;
            case (int)AIListManager.AtkList.WhirlBoostCombo:
                break;
            case (int)AIListManager.AtkList.Ultimate:
                break;
        }
    }

    // 逃走モード処理
    public Vector3 CalcEscMove(Vector3 move,float speed,int actID)
    {
        switch (actID)
        {
            case (int)AIListManager.EscList.Normal:
                break;
            case (int)AIListManager.EscList.Wave:
                break;
            case (int)AIListManager.EscList.HighSpeed:
                break;
            default:
                break;
        }
        return Vector3.zero;
    }

    public Vector3 CalcMovePos(Vector3 originPos, float speed)
    {
        return Vector3.zero;
    }

    public int ChooseAppr(AI_NameListAttack atk)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in atk.apprProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach(KeyValuePair<int,float> elem in atk.apprProbs)
        {
            if(randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
            return 0;
    }

    public int ChooseAppr(AI_NameListDefence def)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in def.apprProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in def.apprProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseWait(AI_NameListAttack atk)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in atk.waitProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in atk.waitProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseWait(AI_NameListDefence def)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in def.waitProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in def.waitProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseWait(AI_NameListLeader leader)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in leader.waitProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in leader.waitProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseWait(AI_NameListBoss boss)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in boss.waitProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in boss.waitProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseAtk(AI_NameListAttack atk)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in atk.atkProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in atk.atkProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseAtk(AI_NameListDefence def)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in def.atkProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in def.atkProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseAtk(AI_NameListLeader leader)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in leader.atkProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in leader.atkProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }
    public int ChooseEsc(AI_NameListAttack atk)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in atk.escProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in atk.escProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }

    public int ChooseEsc(AI_NameListDefence def)
    {
        float total = 0;

        foreach (KeyValuePair<int, float> elem in def.escProbs)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach (KeyValuePair<int, float> elem in def.escProbs)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint = elem.Value;
            }
        }
        return 0;
    }
}
