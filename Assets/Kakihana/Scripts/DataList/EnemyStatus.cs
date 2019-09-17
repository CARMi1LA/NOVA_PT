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

    // 敵の種類
    public enum EnemyType
    {
        Common = 0,     // ザコ敵
        Leader,         // 中ボス
        Boss,           // ボス
        Player          // プレイヤー
    }

    // 敵の役割
    public enum EnemyPosition
    {
        Attack = 0,     // 攻撃タイプ
        Defence,        // 防御タイプ
        Special,        // 特殊タイプ
        Player          // プレイヤー
    }
    [SerializeField] public int ID;
    [SerializeField] public string charaName;

    [SerializeField] public EnemyType enemyType;
    [SerializeField] public EnemyPosition enemyPosition;

    [SerializeField] public int hp = 0;                 // HP
    [SerializeField] public int barrier = 0;            // ダメージを一定量吸収するバリア
    [SerializeField] public int energy = 0;             // エネルギー
    [SerializeField] public int score = 0;              // 消滅時に落とす経験値
    [SerializeField] public int actionAI;               // AIの行動パターン

    [SerializeField] public float moveSpeed = 0.0f;
}

