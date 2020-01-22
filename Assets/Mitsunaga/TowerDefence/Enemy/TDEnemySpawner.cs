using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDEnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<TDEnemyWaveList> enemyWaveList; // Wave数
    [SerializeField]
    List<TDEnemyManager> enemyPrefabList;

    List<TowerManager> towerList = new List<TowerManager>();
    TowerManager towerTarget;

    public List<TDEnemyWave> enemyWave;
    float enemyWaveInterval;

    TDEnemyDataList enemyDataList;

    // マスターからフェイズの情報を取得するため不要になった
    public BoolReactiveProperty isBattleFase = new BoolReactiveProperty(false);

    void Awake()
    {
        enemyDataList = Resources.Load<TDEnemyDataList>("TDEnemyDataList");

        towerList.Add(GameManagement.Instance.redTw);
        towerList.Add(GameManagement.Instance.blueTw);
        towerList.Add(GameManagement.Instance.yellowTw);
        towerList.Add(GameManagement.Instance.greenTw);
    }
    void Start()
    {
        float timeCount = 10.0f; // 敵生成のインターバル
        int waveCount = 0;      // 現在のWave
        int enemyCount = 0;     // Wave内のエネミー生成状況

        // タワーリストのHPを監視する
        foreach (var item in towerList)
        {
            item.towerHp
                .Where(x => x <= 0)
                .Subscribe(_ =>
                {
                    towerList.Remove(item);
                });
        }

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log(GameManagement.Instance.gameState.Value.ToString());

            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(x => !GameManagement.Instance.isPause.Value) // 一時停止用
            .Where(x => GameManagement.Instance.gameState.Value == GameManagement.BattleMode.Attack) // 戦闘フェイズか待機フェイズかの確認
            .Where(x => enemyCount < enemyWave.Count)
            .Subscribe(_ =>
            {
                timeCount += Time.deltaTime;
                if(timeCount >= enemyWaveInterval)
                {
                    timeCount = 0.0f;

                    foreach(var enemy in enemyWave[enemyCount].enemyWavePart)
                    {
                        // enemyWavePartに応じたサイズの敵を生成(タイプはランダムの予定)
                        enemyInstance(enemy);
                    }

                    enemyCount++;
                }

            }).AddTo(this.gameObject);

        GameManagement.Instance.gameState
            .Where(x => x == GameManagement.BattleMode.Attack)
            .Subscribe(_ =>
            {
                // 生成するエネミー量、生成間隔の取得
                enemyWave = enemyWaveList[waveCount].enemyWave;
                enemyWaveInterval = enemyWaveList[waveCount].enemyWaveInterval;
                waveCount++;
                enemyCount = 0;

            }).AddTo(this.gameObject);

        GameManagement.Instance.gameState
            .Where(x => x == GameManagement.BattleMode.Wait)
            .Subscribe(_ =>
            {
                towerTarget = towerList[Random.Range(0, towerList.Count)];

                

            }).AddTo(this.gameObject);
    }

    Transform enemyInstance(TDList.EnemySizeList size)
    {
        // エネミーのタイプと生成するタワーをランダムに指定

        // エネミーを生成
        List<TDEnemyManager> enemyList = new List<TDEnemyManager>();
        enemyList.Clear();
        foreach(var item in enemyPrefabList)
        {
            if(size == item.eSize)
            {
                enemyList.Add(item);
            }
        }
        int pickupEnemyPrefab = Random.Range(0, enemyList.Count);
        TDEnemyManager createEnemy = Instantiate(enemyList[pickupEnemyPrefab]);

        // 初期位置と向きを設定
        int pickupTower = Random.Range(0, 10);
        if (pickupTower < towerList.Count)
        {
            createEnemy.targetTsf = towerList[pickupTower].transform;
            createEnemy.transform.position = towerList[pickupTower].spawnPos.position + new Vector3
                (
                0.5f * Random.Range(-towerList[pickupTower].spawnPos.lossyScale.x, towerList[pickupTower].spawnPos.lossyScale.x), 
                0,
                0.5f * Random.Range(-towerList[pickupTower].spawnPos.lossyScale.z, towerList[pickupTower].spawnPos.lossyScale.z)
                );
        }
        else
        {
            createEnemy.targetTsf = towerTarget.transform;
            createEnemy.transform.position = towerTarget.spawnPos.position + new Vector3
                (
                0.5f * Random.Range(-towerTarget.spawnPos.lossyScale.x, towerTarget.spawnPos.lossyScale.x), 
                0,
                0.5f * Random.Range(-towerTarget.spawnPos.lossyScale.z, towerTarget.spawnPos.lossyScale.z)
                );

        }
        createEnemy.transform.LookAt(Vector3.zero);
        createEnemy.playerTsf = GameManagement.Instance.playerTrans;
        createEnemy.InitEnemyData(enemyDataList.GetEnemyData(createEnemy.eSize, createEnemy.eType));


        // 生成したエネミーのTransformを返す
        return createEnemy.transform;
    }
}
