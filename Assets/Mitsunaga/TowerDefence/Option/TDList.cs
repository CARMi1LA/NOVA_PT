using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDList
{
    public enum ParentList
    {
        Other = 0,
        Player,
        Enemy,
        Turret,
    }
    public enum TowerList
    {
        Blue = 0,
        Red,
        Yellow,
        Green,
    }
    public enum BulletTypeList
    {
        Normal = 0,
        Missile = 1,
        Bomb = 2,
        Boost = 3
    }
    public enum EnemySizeList
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        Extra = 3,
    }
    public enum EnemyTypeList
    {
        Attack = 0,
        Defence = 1,
        Speed = 2,
        Other = 3,
    }
}
