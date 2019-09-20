using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public class EnemySpawner : ESSingleton<EnemySpawner>
{
    public enum SpawnList
    {
        Top = 0,
        Left = 1,
        Right = 2
    }

    [SerializeField] private EnemyPool[] enemyPools;
    [SerializeField] private float spawnOffset;

    [Header("プールをまとめるオブジェクトを作成、格納")]
    [SerializeField] private Transform enemyPoolObj;            // スポーンした敵をまとめるオブジェクトをここに格納

    [Header("自動稼働し、設定する必要がない変数")]
    [SerializeField] private int spawnCount;                    // 現在のスポーン数
    [SerializeField] private float xAbs, zAbs;                  // スポーン先座標の絶対値
    [SerializeField] private float maxR, minR;                  // ホットスポットのスポーン最小範囲と最大範囲を2乗したもの

    [SerializeField] private Vector3[] spawnPos;                  // スポーン先の座標
    [SerializeField] private Transform playerTrans;             // プレイヤーのトランスフォーム
    public ReactiveProperty<EnemyUnitManager> spawnEnemyUnit { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        spawnPos[(int)SpawnList.Top] = new Vector3(
            GameManagement.Instance.playerTrans.position.x,
            GameManagement.Instance.playerTrans.position.y,
            GameManagement.Instance.playerTrans.position.z + spawnOffset
            );
        spawnPos[(int)SpawnList.Left] = new Vector3(
            GameManagement.Instance.playerTrans.position.x - spawnOffset,
            GameManagement.Instance.playerTrans.position.y,
            GameManagement.Instance.playerTrans.position.z + spawnOffset
            );
        spawnPos[(int)SpawnList.Right] = new Vector3(
            GameManagement.Instance.playerTrans.position.x + spawnOffset,
            GameManagement.Instance.playerTrans.position.y,
            GameManagement.Instance.playerTrans.position.z + spawnOffset
            );
        spawnEnemyUnit.Subscribe(_ =>
        {
            Instantiate(spawnEnemyUnit.Value, spawnPos[Random.Range(0,2)], Quaternion.identity);
        });
    }

    public void EnemySpawnUnitSet(EnemyUnitManager enemyUnit)
    {
        spawnEnemyUnit.Value = enemyUnit;
    }
}
