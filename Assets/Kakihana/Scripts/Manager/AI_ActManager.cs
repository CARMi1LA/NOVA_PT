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
