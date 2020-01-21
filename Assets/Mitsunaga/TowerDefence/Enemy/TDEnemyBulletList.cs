using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDEnemyBulletList
{
    float defaultBulletInterval = 1.0f;

    public TDEnemyBullet GetEnemyBullet(TDList.BulletTypeList type)
    {
        TDEnemyBullet eBullet = new TDEnemyBullet();

        switch (type)
        {
            case TDList.BulletTypeList.E_Single:
                {
                    eBullet.bInterval = 1.0f;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_Single, // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                }
                break;
            case TDList.BulletTypeList.E_3Way:
                {
                    eBullet.bInterval = 0.3f;
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_3Way,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 0, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                         );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_3Way,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, -15, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_3Way,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 15, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_3Way,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, -30, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                    eBullet.bDataList.Add
                        (
                            new TDBulletData
                            (
                                TDList.ParentList.Enemy,        // 陣営
                                TDList.BulletTypeList.E_3Way,   // 弾の種類
                                Vector3.zero,                   // 生成位置 + 生成キャラクターの位置
                                new Vector3(0, 30, 0)            // 生成方向 + 生成キャラクターの方向
                            )
                        );
                }
                break;
            case TDList.BulletTypeList.None:
                break;
            default:
                {
                    eBullet.bInterval = defaultBulletInterval;
                    eBullet.bDataList.Add
                        (
                        new TDBulletData
                        (
                            TDList.ParentList.Enemy,        // 陣営
                            TDList.BulletTypeList.Normal,   // 弾の種類
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