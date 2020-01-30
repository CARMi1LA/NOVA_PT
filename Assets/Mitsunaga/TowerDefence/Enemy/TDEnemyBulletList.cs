using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDEnemyBulletList
{
    float defaultBulletInterval = 1.0f;

    int allRangeBulletCount = 16;

    public TDEnemyBullet GetEnemyBullet(TDList.EnemyAttackTypeList type)
    {
        TDEnemyBullet eBullet = new TDEnemyBullet();
        eBullet.bInterval = defaultBulletInterval;

        switch (type)
        {
            case TDList.EnemyAttackTypeList.E_Single:
                {
                    eBullet.bInterval = 1.0f;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal, // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                }
                break;
            case TDList.EnemyAttackTypeList.E_Fast:
                {
                    eBullet.bInterval = 0.3f;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal, // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                }
                break;
            case TDList.EnemyAttackTypeList.E_3Way:
                {
                    eBullet.bInterval = 0.5f;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, -15, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 15, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                }
                break;
            case TDList.EnemyAttackTypeList.E_5Way:
                {
                    eBullet.bInterval = 1.0f;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, -15, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 15, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, -30, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Normal,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 30, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                }
                break;
            case TDList.EnemyAttackTypeList.E_AllRange:
                {
                    eBullet.bInterval = 2.0f;
                    float angle = 360.0f / allRangeBulletCount;
                    for (int i = 0; i < allRangeBulletCount; ++i)
                    {
                        eBullet.bDataList.Add
                            (
                                new TDBulletData
                                (
                                    TDList.ParentList.Enemy,        // 陣営
                                    TDList.BulletTypeList.E_Normal, // 弾の種類
                                    Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                    new Vector3(0, i * angle, 0)    // 生成方向 + 生成キャラクターの方向
                                )
                             );
                    }
                }
                break;
            case TDList.EnemyAttackTypeList.None:
                break;
            default:
                {
                    eBullet.bInterval = defaultBulletInterval;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                            TDList.ParentList.Enemy,        // 陣営
                            TDList.BulletTypeList.E_Normal, // 弾の種類
                            Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                            new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                } // デフォルト：1Way弾
                break;
        }

        return eBullet;
    }
}
public class TDEnemyBullet
{
    // 射撃間隔
    public float bInterval;
    // 発射する弾の方向や速度などのデータ
    public List<TDBulletData> bDataList = new List<TDBulletData>();
}