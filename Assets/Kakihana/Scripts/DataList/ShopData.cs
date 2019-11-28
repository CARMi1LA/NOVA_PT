using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    public ShopPlayerPramData[] shopData_Player;
    public ShopTowerPramData[] redData_Tower, blueData_Tower, yellowData_Tower, greenData_Tower;
    public LevelData_Player levelData_Player;
}