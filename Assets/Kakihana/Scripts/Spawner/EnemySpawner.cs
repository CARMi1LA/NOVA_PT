using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public class EnemySpawner : ESSingleton<EnemySpawner>
{

    [SerializeField] private EnemyPool enemyPool;

    [Header("プールをまとめるオブジェクトを作成、格納")]
    [SerializeField] private Transform enemyPoolObj;            // スポーンした敵をまとめるオブジェクトをここに格納

    [Header("自動稼働し、設定する必要がない変数")]
    [SerializeField] private int spawnCount;                    // 現在のスポーン数
    [SerializeField] private float xAbs, zAbs;                  // スポーン先座標の絶対値
    [SerializeField] private float maxR, minR;                  // ホットスポットのスポーン最小範囲と最大範囲を2乗したもの

    [SerializeField] private Vector3 spawnPos;                  // スポーン先の座標
    [SerializeField] private Transform playerTrans;             // プレイヤーのトランスフォーム
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
