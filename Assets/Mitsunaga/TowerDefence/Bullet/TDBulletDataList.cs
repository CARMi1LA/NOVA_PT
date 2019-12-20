using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TDBulletDataList", menuName = "TDScriptable/TDBulletDataList", order = 0)]

public class TDBulletDataList : ScriptableObject
{
    public float bSpeed = 250.0f;       // 弾のスピード
    public float bDeathCount = 2.0f;    // 自動消滅までの時間 (単位：秒)
    public int bDamage = 1;             // 弾のダメージ量 (基本的に1)
}
