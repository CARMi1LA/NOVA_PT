using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetting : MonoBehaviour
{
    public enum BulletList
    {
        Normal = 0,                        // 通常の弾を撃つ
        Scatter = 1,                       // 多方向に向けて弾を撃つ
        Fireworks = 2,                     // 全方向に向けて弾を撃つ
        Booster = 3,                       // 一定の方向に弾を撃ち、一定距離までゆっくり進んだ後、分散し高速化する
        None = 4,                          // 攻撃しない
        Whirlpool = 5,                     // 渦巻状に弾を発射
        Forrow = 6,                        // プレイヤーを追尾する弾を発射
                                           // 中ボスクラス専用攻撃
        WhirlScatterCombo = 11,            // Whirlpool&Scatterの同時攻撃
        FireworksCombo = 12,               // 弾速が遅いFireworks発射後、角度を変えて弾速が早いFireworksを発射
        UltMegaFireworks = 13,             // Fireworksを多数生成
                                           // ここからボス専用攻撃 一定体力低下時にのみ実行 //
        WhirlFireCombo = 21,               // WhirlpoolとFireworksの同時攻撃
        BoostFireCombo = 22,               // Boosterの弾ををFireworks状に攻撃
        WhirlBoostCombo = 23,              // WhirlpoolとBoosterの同時攻撃
        Ultimate = 24                      // DPSチェックによる攻撃、弾を4発召喚し、一定時間内にダメージを与えないと大爆発
    }

    [SerializeField] protected BulletList bulletList;

    [SerializeField] protected float shootInterval;

    [SerializeField] protected BulletList[] leaderIndex = new BulletList[] { BulletList.WhirlScatterCombo, BulletList.FireworksCombo, BulletList.UltMegaFireworks };
    [SerializeField] protected BulletList[] bossIndex = new BulletList[] { BulletList.WhirlFireCombo, BulletList.BoostFireCombo, BulletList.WhirlBoostCombo };

    // Start is called before the first frame update
    void Start()
    {

    }

    protected void IntervalSet(BulletList list)
    {
        switch (bulletList)
        {
            case BulletList.Normal:
                shootInterval = 0.1f;
                break;
            case BulletList.Scatter:
                shootInterval = 0.2f;
                break;
            case BulletList.Fireworks:
                shootInterval = 0.2f;
                break;
            case BulletList.Booster:
                shootInterval = 0.25f;
                break;
            case BulletList.Whirlpool:
                shootInterval = 0.25f;
                break;
            case BulletList.Forrow:
                shootInterval = 0.5f;
                break;
            default:
                shootInterval = 3.0f;
                break;
        }
    }
}
