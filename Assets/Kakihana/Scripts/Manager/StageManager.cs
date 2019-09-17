using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class StageManager : SMSingleton<StageManager>
{
    // ステージ管理

    [SerializeField] private int stageNum;

    [SerializeField] private StageDataList dataList;
    [SerializeField] private StageData stageData;

    [SerializeField] IntReactiveProperty nowWave = new IntReactiveProperty(0);
    [SerializeField] private int maxWave;
    [SerializeField] IntReactiveProperty enemyAliveNum = new IntReactiveProperty(0);

    public bool eventFlg;             // イベントが発生するステージかどうか
    public BoolReactiveProperty nextWaveFlg = new BoolReactiveProperty(false); // ウェーブ進行準備完了フラグ

    public StageManager()
    {
        dataList = Resources.Load<StageDataList>("StageDataList");
        stageData = dataList.stageDataList[stageNum];
        maxWave = stageData.waveType.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextWaveFlg.Where(_ => nextWaveFlg.Value == true).Subscribe(_ =>
        {
            nowWave.Value++;
            nextWaveFlg.Value = false;
        }).AddTo(this.gameObject);

        enemyAliveNum.Where(_ => enemyAliveNum.Value <= 0).Subscribe(_ =>
        {
            nextWaveFlg.Value = true;
        }).AddTo(this.gameObject);

        nowWave.Subscribe(_ =>
        {
            switch (stageData.waveType[nowWave.Value - 1])
            {
                case StageData.WaveType.Fixed:
                    var nextSpawnEnemys = stageData.waveEnemyObj[stageData.waveTable[nowWave.Value - 1]];
                    enemyAliveNum.Value = nextSpawnEnemys.unitEnemys.Length;
                    EnemySpawner.Instance.EnemySpawnUnitSet(nextSpawnEnemys);
                    break;
                case StageData.WaveType.Random:
                    nextSpawnEnemys = stageData.waveEnemyObj[Random.Range(0, stageData.waveEnemyObj.Length - 1)];
                    enemyAliveNum.Value = nextSpawnEnemys.unitEnemys.Length;
                    EnemySpawner.Instance.EnemySpawnUnitSet(nextSpawnEnemys);
                    break;
                case StageData.WaveType.Select:
                    break;
                case StageData.WaveType.Event:
                    break;
                case StageData.WaveType.Boss:
                    break;
            }  
        }).AddTo(this.gameObject);
    }

    public void EnemyDestroy()
    {
        enemyAliveNum.Value--;
    }
}
