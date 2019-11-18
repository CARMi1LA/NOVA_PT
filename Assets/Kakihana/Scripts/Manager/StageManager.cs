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

    // ステージの行動状態
    public enum StageWaveAction
    {
        WaveWaiting = 0,        // ウェーブ準備中
        WaveCreate  = 1,        // ウェーブ準備完了
        WavePlaying = 2,        // ウェーブ進行中
    }

    public enum SpawnList
    {
        Left = 0,
        Right = 1
    }

    // ステージID
    [SerializeField] private int stageID;

    // ステージデータが格納されているデータリスト
    [SerializeField] private StageDataList dataList;
    // 各ステージのデータ
    [SerializeField] public StageData stageData;
    // スポナークラス
    [SerializeField] private EnemySpawner enemySpawner;

    // 現在のウェーブ
    [SerializeField] public IntReactiveProperty nowWave = new IntReactiveProperty(0);
    // 最大ウェーブ数
    [SerializeField] private int maxWave;
    // 現在のウェーブで生存している敵の数
    [SerializeField] IntReactiveProperty enemyAliveNum = new IntReactiveProperty(0);

    [SerializeField] private float spawnOffset;
    [SerializeField] public bool eventFlg;                                         // イベントが発生するステージかどうか
    [SerializeField] private Vector3[] spawnPos;                  // スポーン先の座標
    [SerializeField] private Transform playerTrans;               // プレイヤーのトランスフォーム
    public BoolReactiveProperty nextWaveFlg = new BoolReactiveProperty(false); // ウェーブ進行準備完了フラグ
    [SerializeField] public BoolReactiveProperty startingFlg = new BoolReactiveProperty(false);
    [SerializeField] private WaveActionReactiveProperty waveAct = new WaveActionReactiveProperty();

    // 参照用のカスタムプロパティ
    [SerializeField]
    public IReadOnlyReactiveProperty<StageWaveAction> enemyAIPropaty
    {
        get { return waveAct; }
    }

    protected override void Awake()
    {
        base.Awake();
        playerTrans = GameManagement.Instance.playerTrans;
        nowWave.Value = 0;
        // データリストの取得
        dataList = Resources.Load<StageDataList>("StageDataList");
        if (stageID == 0 || stageID >= 10)
        {
            switch (stageID)
            {
                case 0:
                    // IDより各ステージデータの取得
                    stageData = dataList.stageDataList[0];
                    break;
                case 11:
                    // IDより各ステージデータの取得
                    stageData = dataList.stageDataList[4];
                    break;
            }
        }
        else
        {
            // IDより各ステージデータの取得
            stageData = dataList.stageDataList[stageID];
        }
        // 最大ウェーブ数の取得
        maxWave = stageData.waveType.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始直前に実行
        this.UpdateAsObservable()
            .Where(s => startingFlg.Value == false && GameManagement.Instance.isPause.Value == false)
            .Sample(TimeSpan.FromSeconds(3.0f))
            .Subscribe(s => 
            {
                // ゲームが開始してから3秒後に敵が出現するように
                startingFlg.Value = true;
                nextWaveFlg.Value = true;
                waveAct.Value = StageWaveAction.WaveWaiting;
            }).AddTo(this.gameObject);

        // ゲームが開始したら実行
        startingFlg.Where(s => s == true)
            .Where(s => GameManagement.Instance.starting.Value == true)
            .Subscribe(startingFlg => 
            {
                waveAct.Where(w => w == StageWaveAction.WaveWaiting)
                .Sample(TimeSpan.FromSeconds(3.0f))
                .Subscribe(w => 
                {
                    if (nextWaveFlg.Value == true)
                    {
                        nowWave.Value++;
                        waveAct.Value = StageWaveAction.WaveCreate;
                        nextWaveFlg.Value = false;
                    }
                    
                   
                    //// 次ウェーブ移行イベント
                    //nowWave.Subscribe(_ =>
                    //{

                    //}).AddTo(this.gameObject);
                }).AddTo(this.gameObject);

                waveAct.Where(w => w == StageWaveAction.WaveCreate)
                .Sample(TimeSpan.FromSeconds(3.0f))
                .Subscribe(w => 
                {
                    // ウェーブで出現する敵パターンの設定
                    switch (stageData.waveType[nowWave.Value - 1])
                    {
                        // 固定出現
                        case StageData.WaveType.Fixed:
                            enemyAliveNum.Value = stageData.waveEnemyObj[stageData.waveTable[nowWave.Value - 1]].unitEnemys.Length;
                            EnemyUnitSpawn(stageData.waveTable[nowWave.Value - 1]);
                            waveAct.Value = StageWaveAction.WavePlaying;
                            break;
                        // ランダム出現
                        case StageData.WaveType.Random:
                            int seed = Random.Range(0, stageData.waveEnemyObj.Length - 1);
                            enemyAliveNum.Value = stageData.waveEnemyObj[seed].unitEnemys.Length;
                            waveAct.Value = StageWaveAction.WavePlaying;
                            break;
                        case StageData.WaveType.Select:
                            break;
                        case StageData.WaveType.Event:
                            break;
                        // ボス出現（最終ウェーブ）
                        case StageData.WaveType.Boss:
                            enemyAliveNum.Value = stageData.waveEnemyObj[maxWave - 1].unitEnemys.Length;
                            EnemyUnitSpawn(maxWave);
                            waveAct.Value = StageWaveAction.WavePlaying;
                            break;
                    }
                }).AddTo(this.gameObject);

                waveAct.Where(w => w == StageWaveAction.WavePlaying)
                .Subscribe(w =>
                {
                    // 敵が全滅したら次のウェーブへ
                    enemyAliveNum.Where(_ => enemyAliveNum.Value <= 0).Subscribe(_ =>
                    {
                        nextWaveFlg.Value = true;
                        waveAct.Value = StageWaveAction.WaveWaiting;
                    }).AddTo(this.gameObject);
                }).AddTo(this.gameObject);
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => GameManagement.Instance.isPause.Value == false)
            .Sample(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {

            }).AddTo(this.gameObject);

        nowWave.Where(_ => nowWave.Value >= maxWave && enemyAliveNum.Value <= 0)
            .Subscribe(_ =>
            {
                GameManagement.Instance.isClear.Value = true;
            }).AddTo(this.gameObject);
    }

    // 敵消滅メソッド
    public void EnemyDestroy(EnemyManager enemy)
    {
        if (enemy != null)
        {
            // 現在のウェーブの敵生存数を減らす
            enemyAliveNum.Value--;
            GameManagement.Instance.DestoyScore();
            for (int i = 0; i < 5; i++)
            {
                new ItemData(enemy.enemyStatus.score / 5, 0, 0, ItemManager.ItemType.Score, enemy.transform.position);
            }
            Destroy(enemy.gameObject);
        }
    }

    public void EnemyUnitSpawn(int index)
    {
        int rand = Random.Range(0, 2);
        spawnPos[(int)SpawnList.Left] = new Vector3(
            GameManagement.Instance.playerTrans.position.x - spawnOffset,
            GameManagement.Instance.playerTrans.position.y,
            GameManagement.Instance.playerTrans.position.z + Random.Range(-40, 40)
            );
        spawnPos[(int)SpawnList.Right] = new Vector3(
            GameManagement.Instance.playerTrans.position.x + spawnOffset,
            GameManagement.Instance.playerTrans.position.y,
            GameManagement.Instance.playerTrans.position.z + Random.Range(-40, 40)
            );
        Instantiate(stageData.waveEnemyObj[index], spawnPos[rand], Quaternion.identity);
        Debug.Log(rand);
    }

    [System.Serializable]
    public class WaveActionReactiveProperty : ReactiveProperty<StageManager.StageWaveAction>
    {
        public WaveActionReactiveProperty() { }
        public WaveActionReactiveProperty(StageManager.StageWaveAction initialValue) : base(initialValue) { }
    }
}
