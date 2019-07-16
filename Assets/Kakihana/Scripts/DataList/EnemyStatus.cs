using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[System.Serializable]
public class EnemyStatus
{
    /*
     出現するオブジェクトのパラメータをまとめたクラス
     （EnemyStatusと書いてあるがプレイヤーの情報もここでまとめる）
    */

    [SerializeField] public int hp = 0;                 // HP
    [SerializeField] public int barrier = 0;            // ダメージを一定量吸収するバリア
    [SerializeField] public int atk = 0;                // 攻撃力
    [SerializeField] public int score = 0;                // 消滅時に落とす経験値

    [SerializeField] public float moveSpeed = 0.0f;
}

