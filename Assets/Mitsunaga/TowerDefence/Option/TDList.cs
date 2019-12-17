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
}
