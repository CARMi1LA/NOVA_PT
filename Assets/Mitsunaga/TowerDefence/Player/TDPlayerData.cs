using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TDPlayerData
{
    // 陣営データ
    public TDList.ParentList pParent = TDList.ParentList.Player;

    // ヘルスゲージ           最大 / 現在
    public int pMaxHealth = 10;
    public IntReactiveProperty pHealth = new IntReactiveProperty();

    // エネルギーゲージ       最大 / 現在
    public float pMaxEnergy = 100.0f;
    public FloatReactiveProperty pEnergy = new FloatReactiveProperty();

    // アルティメットゲージ   最大 / 現在
    public float pMaxUltimate = 100;
    public FloatReactiveProperty pUltimate = new FloatReactiveProperty();

    // 通常攻撃
    public int pAttackInterval = 6;     // 通常攻撃の発動間隔(単位：フレーム)

    // スキル
    public enum SkillTypeList
    {
        Sword   = 0,
        Razer   = 1,
        Missile = 2,
        Bomb    = 3
    }
    public SkillTypeList pSkillType = SkillTypeList.Sword;    // スキルの型
    public float pSkillCost = 20.0f;        // スキルの発動コスト
    public float pSkillInterval = 1.0f;         // スキルの発動間隔(単位：フレーム)

    // アルティメット
    public enum UltimateTypeList
    {
        Strong  = 0,
        Heal    = 1,
        Bomb    = 2,
        Slow    = 3
    }
    public UltimateTypeList pUltimateType = 0;  // アルティメットの型

    // 移動
    public float pSpeed = 70;  // 移動速度
    public float pSpeedMul = 10;
    // ダッシュ
    public float pDashCost = 20.0f;      // 回避の発動コスト
    public float pDashTime = 0.5f;      // 回避の行動時間

    // ダメージの無効化
    public bool pBarrier = false;   // ダメージ無効時間の有効化

    // 初期化
    public TDPlayerData()
    {
        pHealth.Value   = pMaxHealth;
        pEnergy.Value   = pMaxEnergy;
        pUltimate.Value = pMaxUltimate;
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
    public void SetShotInterval(int shotInterval)
    {
        pAttackInterval = shotInterval;
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
