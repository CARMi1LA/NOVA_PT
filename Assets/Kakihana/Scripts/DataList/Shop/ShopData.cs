using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    // ショップ運営に必要なデータが格納されているクラス
    // ScriptableObjectからこのデータ群を編集できる

    // プレイヤー強化のパラメータ参照用リスト
    public enum Player_ParamList
    {
        Param_HP = 0,
        Param_Speed = 1,
        Param_Interval = 2
    }

    // タワー強化（赤）のパラメータ参照用リスト
    public enum TowerRed_ParamList
    {
        Param_Trap = 0,
        Param_Turret = 1,
        Param_Tower = 2,
    }

    // タワー強化（青）のパラメータ参照用リスト
    public enum TowerBlue_ParamList
    {
        Param_Trap = 0,
        Param_Turret = 1,
        Param_Tower = 2,
    }

    // タワー強化（黄）のパラメータ参照用リスト
    public enum TowerYellow_ParamList
    {
        Param_Trap = 0,
        Param_Turret = 1,
        Param_Tower = 2,
    }

    // タワー強化（緑）のパラメータ参照用リスト
    public enum TowerGreen_ParamList
    {
        Param_Trap = 0,
        Param_Turret = 1,
        Param_Tower = 2,
    }

    // スキル変更のパラメータ参照用リスト
    public enum Skill_ParamList
    {
        Normal = 0,
        Razer = 1,
        Missile = 2,
        Bomb = 3
    }

    // Ult変更のパラメータ参照用リスト
    public enum Ult_ParamList
    {
        Normal = 0,
        Trap = 1,
        Bomb = 2,
        Repair = 3
    }
    // プレイヤー強化などのデータが格納されているクラス
    [Header("プレイヤー強化のパラメータ")]
    public ShopPlayerPramData[] shopData_Player;
    // タワー強化などのデータが格納されているクラス
    [Header("タワー強化のパラメータ")]
    public ShopTowerPramData[] redData_Tower, blueData_Tower, yellowData_Tower, greenData_Tower;
    // Ult変更などのデータが格納されているクラス
    [Header("Ult変更のパラメータ")]
    public ShopUltPramData[] shopData_Ult;
    // スキル変更などのデータが格納されているクラス
    [Header("スキル変更のパラメータ")]
    public ShopSkillParamData[] shopData_Skill;
    // プレイヤー強化のパラメータごとのレベルが格納されているクラス
    [Header("パラメータごとのレベル（設定不要）")]
    public LevelData_Player levelData_Player;
    // タワー強化のパラメータごとのレベルが格納されているクラス
    public LevelData_Tower levelData_Tower;
    // Ult変更のパラメータごとのレベルが格納されているクラス
    public LevelData_Ult levelData_Ult;
    // スキル変更のパラメータごとのレベルが格納されているクラス
    public LevelData_Skill levelData_Skill;
}