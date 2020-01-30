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

    bool isSkiped = false;

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
            .Where(x => !GameManagement.Instance.isPause.Value) // 一時停止用
            .Where(x => GameManagement.Instance.gameState.Value == GameManagement.BattleMode.Attack) // 戦闘フェイズか待機フェイズかの確認
            .Where(x => enemyCount < enemyWave.Count)
            .Subscribe(_ =>
            {
                isSkiped = false;
                timeCount += Time.deltaTime;
                if(timeCount >= enemyWaveInterval)
                {
                    timeCount = 0.0f;

                    foreach(var enemy in enemyWave[enemyCount].enemyWavePart)
                    {
                        // enemyWavePartに応じたサイズの敵を生成(タイプはランダムの予定)
                        GameManagement.Instance.enemyInfoAdd.OnNext(enemyInstance(enemy));
                    }

                    enemyCount++;
                }

            }).AddTo(this.gameObject);

        // エネミーが全員消滅したらタイムをスキップする
        this.UpdateAsObservable()
            .Where(x => !isSkiped)
            .Where(x => !GameManagement.Instance.isPause.Value) // 一時停止用
            .Where(x => GameManagement.Instance.gameState.Value == GameManagement.BattleMode.Attack) // 戦闘フェイズか待機フェイズかの確認
            .Where(x => enemyCount >= enemyWave.Count)
            .Where(x => GameManagement.Instance.enemyInfoList.enemyInfo.Count == 0)
            .Subscribe(_ =>
            {
                isSkiped = true;
                GameManagement.Instance.masterTimeSkip.OnNext(Unit.Default);

            }).AddTo(this.gameObject);
        // 戦闘フェイズに変わったら
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
        // 準備フェイズに変わったら
        GameManagement.Instance.gameState
            .Where(x => x == GameManagement.BattleMode.Wait)
            .Subscribe(_ =>
            {
                towerTarget = towerList[Random.Range(0, towerList.Count)];

                if(towerTarget.towerColor == ShopData.TowerColor.Blue)
                {
                    GameManagement.Instance.targetTw = MasterData.TowerColor.Blue;
                }
                else if(towerTarget.towerColor == ShopData.TowerColor.Red)
                {
                    GameManagement.Instance.targetTw = MasterData.TowerColor.Red;
                }
                else if(towerTarget.towerColor == ShopData.TowerColor.Yellow)
                {
                    GameManagement.Instance.targetTw = MasterData.TowerColor.Yellow;
                }
                else if(towerTarget.towerColor == ShopData.TowerColor.Green)
                {
                    GameManagement.Instance.targetTw = MasterData.TowerColor.Green;
                }

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
        if (pickupTower < towerList.Count && size != TDList.EnemySizeList.Extra)
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
        createEnemy.transform.LookAt(new Vector3(0, createEnemy.transform.position.y, 0));
        createEnemy.playerTsf = GameManagement.Instance.playerTrans;
        createEnemy.InitEnemyData(enemyDataList.GetEnemyData(createEnemy.eSize, createEnemy.eType));


        // 生成したエネミーのTransformを返す
        return createEnemy.transform;
    }
}
