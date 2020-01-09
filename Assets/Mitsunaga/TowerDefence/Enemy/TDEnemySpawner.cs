using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDEnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<TDEnemyWaveList> enemyWaveList; // Wave数

    List<TDEnemyWave> enemyWave;
    float enemyWaveInterval;

    public BoolReactiveProperty isBattleFase = new BoolReactiveProperty(false);

    void Awake()
    {
        
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
    }
}
