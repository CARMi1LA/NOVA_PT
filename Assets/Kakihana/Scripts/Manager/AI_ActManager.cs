using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ActManager : MonoBehaviour
{
    // AIステートをもとに敵の移動量などを計算するクラス

    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    public int[] bossAtkList = new int[] {2,3,5,8,9,10};
    public int[] leaderAtkList = new int[] { 11, 12, 13 };

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
            case (int)AIListManager.ApprList.EnemyGuard:
                break;
        }
        return Vector3.zero;
    }

    // 待機モード処理
    public float ActWaitCalc(int actID)
    {
        float mag = 1.0f;
        switch (actID)
        {
            case (int)AIListManager.WaitList.Normal:
                return mag;
            case (int)AIListManager.WaitList.Quick:
                return mag * 0.5f;
        }
        return mag;
    }

    // 攻撃モード処理
    public void EnemyAtkCalc(Transform origin,int actID,float atkTime)
    {
        float deg = 360.0f;
        float bulletSpeed = 10.0f;
        switch (actID)
        {
            case (int)AIListManager.AtkList.Normal:
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 0.0f);
                break;
            case (int)AIListManager.AtkList.Scatter:
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 0.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, 30.0f);
                new BulletData(bulletSpeed, origin, BulletManager.ShootChara.Enemy, actID, -30.0f);
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
        // 処理は接近モード処理とほぼ同じ
        switch (actID)
        {
            case (int)AIListManager.EscList.Normal:
                Vector3 dif = playerTrans.position - move;
                float radian = Mathf.Atan2(dif.z, dif.x);
                return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * speed * 10;
            case (int)AIListManager.EscList.HighSpeed:
                dif = playerTrans.position - move;
                radian = Mathf.Atan2(dif.z, dif.x);
                return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * (speed * 2) * 10;
            default:
                break;
        }
        return Vector3.zero;
    }

    public Vector3 CalcMovePos(Vector3 originPos, float speed)
    {
        return Vector3.zero;
    }

    // プレイヤー間の距離の計算を行うメソッド
    public float CalcDistance(Vector3 enemyPos)
    {
        return (playerTrans.position - enemyPos).sqrMagnitude;
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
