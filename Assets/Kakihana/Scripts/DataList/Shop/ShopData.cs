using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    // ショップ運営に必要なデータが格納されているクラス
    // ScriptableObjectからこのデータ群を編集できる

    // プレイヤー強化などのデータが格納されているクラス
    [Header("プレイヤー強化のパラメータ")]
    public ShopPlayerPramData[] shopData_Player;
    // タワー強化などのデータが格納されているクラス
    [Header("タワー強化のパラメータ")]
    public ShopTowerPramData[] redData_Tower, blueData_Tower, yellowData_Tower, greenData_Tower;
    // スキル＆Ult変更などのデータが格納されているクラス
    [Header("スキル＆Ult変更のパラメータ")]
    public ShopUltPramData[] shopData_Ult;
    // プレイヤー強化のパラメータごとのレベルが格納されているクラス
    [Header("パラメータごとのレベル（設定不要）")]
    public LevelData_Player levelData_Player;
    // タワー強化のパラメータごとのレベルが格納されているクラス
    public LevelData_Tower levelData_Tower;
    // スキル＆Ult変更のパラメータごとのレベルが格納されているクラス
    public LevelData_Ult levelData_Ult;
}