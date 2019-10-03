using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

// Unity側のランダム関数を使用
using Random = UnityEngine.Random;

public class StageManager : SMSingleton<StageManager>
{
    // ステージ管理クラス

    // ステージID
    [SerializeField] private int stageID;

    // ステージデータが格納されているデータリスト
    [SerializeField] private StageDataList dataList;
    // 各ステージのデータ
    [SerializeField] private StageData stageData;

    // 現在のウェーブ
    [SerializeField] IntReactiveProperty nowWave = new IntReactiveProperty(0);
    // 最大ウェーブ数
    [SerializeField] private int maxWave;
    // 現在のウェーブで生存している敵の数
    [SerializeField] IntReactiveProperty enemyAliveNum = new IntReactiveProperty(0);

    public bool eventFlg;             // イベントが発生するステージかどうか
    public BoolReactiveProperty nextWaveFlg = new BoolReactiveProperty(false); // ウェーブ進行準備完了フラグ
    private BoolReactiveProperty startingFlg = new BoolReactiveProperty(false);

    protected override void Awake()
    {
        base.Awake();

        // データリストの取得
        dataList = Resources.Load<StageDataList>("StageDataList");
        // IDより各ステージデータの取得
        stageData = dataList.stageDataList[stageID];
        // 最大ウェーブ数の取得
        maxWave = stageData.waveType.Length;
        // デバッグ用 読み込み完了エラー処理
        if (dataList == null)
        {
            // エラー、データリストそのものが読み取れていない
            Debug.LogError("DataListNotFound");
        }else if(stageData == null)
        {
            // エラー、データリストは読み込めているがステージのデータが読み込めていない
            Debug.LogError("StageDataNotFound");
        }
        else
        {
            // ステージデータ読み込み完了
            Debug.Log("StageDataLoadSuccess!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable()
            .Where(s => startingFlg.Value == false && GameManagement.Instance.starting.Value == false)
            .Sample(TimeSpan.FromSeconds(10.0f))
            .Subscribe(s => 
            {
                startingFlg.Value = true;
            }).AddTo(this.gameObject);
        this.UpdateAsObservable()
            .Where(s => startingFlg.Value == true && GameManagement.Instance.starting.Value == true)
            .Subscribe(s => 
            {
                // 次ウェーブ移行イベント
                nextWaveFlg.Where(_ => nextWaveFlg.Value == true).Subscribe(_ =>
                {
                    nowWave.Value++;
                    nextWaveFlg.Value = false;
                }).AddTo(this.gameObject);

                // 敵が全滅したら次のウェーブへ
                enemyAliveNum.Where(_ => enemyAliveNum.Value <= 0).Subscribe(_ =>
                {
                    nextWaveFlg.Value = true;
                }).AddTo(this.gameObject);

                // ウェーブの処理
                // 各種ステートに基づき、敵の設定と生存数の設定を行う
                nowWave.Subscribe(_ =>
                {
                    switch (stageData.waveType[nowWave.Value - 1])
                    {
                        // 固定出現
                        case StageData.WaveType.Fixed:
                            var nextSpawnEnemys = stageData.waveEnemyObj[stageData.waveTable[nowWave.Value - 1]];
                            enemyAliveNum.Value = nextSpawnEnemys.unitEnemys.Length;
                            EnemySpawner.Instance.EnemySpawnUnitSet(nextSpawnEnemys);
                            break;
                        // ランダム出現
                        case StageData.WaveType.Random:
                            nextSpawnEnemys = stageData.waveEnemyObj[Random.Range(0, stageData.waveEnemyObj.Length - 1)];
                            enemyAliveNum.Value = nextSpawnEnemys.unitEnemys.Length;
                            EnemySpawner.Instance.EnemySpawnUnitSet(nextSpawnEnemys);
                            break;
                        case StageData.WaveType.Select:
                            break;
                        case StageData.WaveType.Event:
                            break;
                        // ボス出現（最終ウェーブ）
                        case StageData.WaveType.Boss:
                            nextSpawnEnemys = stageData.waveEnemyObj[stageData.waveTable[maxWave - 1]];
                            enemyAliveNum.Value = nextSpawnEnemys.unitEnemys.Length;
                            EnemySpawner.Instance.EnemySpawnUnitSet(nextSpawnEnemys);
                            break;
                    }
                }).AddTo(this.gameObject);
            }).AddTo(this.gameObject);
        
    }

    // 敵消滅メソッド
    public void EnemyDestroy()
    {
        // 現在のウェーブの敵生存数を減らす
        enemyAliveNum.Value--;
    }
}
