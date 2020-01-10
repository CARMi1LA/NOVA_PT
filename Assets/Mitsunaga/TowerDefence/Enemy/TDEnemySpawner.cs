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

    List<TDEnemyWave> enemyWave;
    float enemyWaveInterval;

    TDEnemyDataList enemyDataList;

    /// <summary>
    /// タワーの情報を取得する(準備フェイズ開始時)
    /// 
    /// 1．生存しているタワーのカラー、それぞれの位置を取得
    /// 2．敵を集中させるタワーのカラーを取得
    /// 3．敵を集中させるタワーのカラーをマップ上に反映させる
    /// </summary>

    // マスターからフェイズの情報を取得するため不要になった
    public BoolReactiveProperty isBattleFase = new BoolReactiveProperty(false);

    void Awake()
    {
        enemyDataList = Resources.Load<TDEnemyDataList>("TDEnemyDataList");
    }
    void Start()
    {
        float timeCount = 0.0f; // 敵生成のインターバル
        int waveCount = 0;      // 現在のWave

        this.UpdateAsObservable()
            .Where(x => true) // 一時停止用
            .Where(x => isBattleFase.Value) // 戦闘フェイズか待機フェイズかの確認
            .Subscribe(_ =>
            {
                timeCount += Time.deltaTime;
                if(timeCount >= enemyWaveInterval)
                {
                    timeCount = 0.0f;

                    foreach(var enemy in enemyWave[0].enemyWavePart)
                    {
                        // enemyWavePartに応じたサイズの敵を生成(タイプはランダムの予定)
                        Debug.Log(enemy.ToString());

                        enemyInstance(enemy);
                    }
                    enemyWave.RemoveAt(0);
                }
                if (enemyWave.Count == 0)
                {
                    isBattleFase.Value = false;
                }

            }).AddTo(this.gameObject);

        // 戦闘フェイズに切り替わるたびにWaveデータを読み込む
        isBattleFase
            .Where(x => x)
            .Subscribe(_ =>
            {
                enemyWave = enemyWaveList[waveCount].enemyWave;
                enemyWaveInterval = enemyWaveList[waveCount].enemyWaveInterval;
                waveCount++;

            }).AddTo(this.gameObject);
        // 準備フェイズに切り替わるたびにタワーデータを読み込む
        isBattleFase
            .Where(x => !x)
            .Subscribe(_ =>
            {
                // 生存しているタワーの情報を取得
                
                // その中から標的にするタワーを設定

            }).AddTo(this.gameObject);
    }

    Transform enemyInstance(TDList.EnemySizeList size)
    {
        // エネミーのタイプと生成するタワーをランダムに指定

        // エネミーを生成
        GameObject enemy = Instantiate(enemyPrefabList[0].gameObject);
        // 初期位置と向きを設定

        // 生成したエネミーのTransformを返す
        return enemy.transform;
    }
}
