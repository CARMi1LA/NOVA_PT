using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum AI_Idle
    {

    }

    public enum AI_Approach
    {
        Normal = 0,
        Wave,
        WayPoint,
        HighSpeed
    }

    public enum AI_Wait
    {
        Normal = 0,
        Follow,

    }

    public enum AI_Attack
    {
        Normal = 0,
        Burst,
        Scatter,
    }

    public enum AI_Escape
    {
        Normal = 0,
        Wave,
        WayPoint,
        HighSpeed
    }
    public AI_Idle

    public AI_Approach[] approach;
    public AI_Wait[] wait;
    public AI_Attack[] attack;
    public AI_Escape[] escape;
}
