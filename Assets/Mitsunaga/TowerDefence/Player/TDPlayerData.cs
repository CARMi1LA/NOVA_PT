using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TDPlayerData
{

    // ヘルス　　　　　最大 / 現在
    public int pMaxHealth = 10;
    public IntReactiveProperty pHealth = new IntReactiveProperty();

    // エネルギー　　　最大 / 現在
    public int pMaxEnergy = 100;
    public IntReactiveProperty pEnergy = new IntReactiveProperty();

    // アルティメット　最大 / 現在
    public int pMaxUltimate = 100;
    public IntReactiveProperty pUltimate = new IntReactiveProperty();

    public enum SkillTypeList
    {
        Sword   = 0,
        Razer   = 1,
        Missile = 2,
        Bomb    = 3
    }
    public enum UltimateTypeList
    {
        Strong  = 0,
        Heal    = 1,
        Bomb    = 2,
        Slow    = 3
    }

    public SkillTypeList pSkillType = 0;        // スキルの型
    public UltimateTypeList pUltimateType = 0;  // アルティメットの型

    public float pSpeed;        // 移動速度
    public float pShotInterval; // 射撃間隔

    // 初期化
    public TDPlayerData()
    {
        pHealth.Value   = pMaxHealth;
        pEnergy.Value   = pMaxEnergy;
        pUltimate.Value = 0;
    }

    // 最大ヘルスの変更
    public void SetMaxHealth(int maxHealth)
    {
        // 最大ヘルスの上昇値分、現在ヘルスを回復する
        int hm = maxHealth - pMaxHealth;

        pMaxHealth = maxHealth;
        pHealth.Value += hm;
    }
    // 移動速度の変更
    public void SetSpeed(int speed)
    {
        pSpeed = speed;
    }
    // 射撃間隔の変更
    public void SetShotInterval(float shotInterval)
    {
        pShotInterval = shotInterval;
    }
    // スキルの型の変更
    public void SetSkillType(SkillTypeList sType)
    {
        pSkillType = sType;
    }
    // アルティメットの型の変更
    public void SetUltType(UltimateTypeList uType)
    {
        pUltimateType = uType;
    }
}
