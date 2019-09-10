using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AIListManager : MonoBehaviour
{
    // AIのステート名称を格納しているクラス
    public enum AI_Idle
    {
        Normal = 0
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

    public AI_Idle[] idle;

    public AI_Approach[] approach;
    public AI_Wait[] wait;
    public AI_Attack[] attack;
    public AI_Escape[] escape;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}

public class AIList_Attack
{
    public enum AI_Idle
    {
        Normal = 0
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
}

public class AIList_Defence
{
    public enum AI_Idle
    {
        Normal = 0
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
}

public class AIList_Speed
{
    public enum AI_Idle
    {
        Normal = 0
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
}
public class AIList_ALL
{
    public enum AI_Idle
    {
        Normal = 0,
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
        Wave
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
}