using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class StageManager : MonoBehaviour
{
    // ステージ管理

    [SerializeField] private int stageNum;

    [SerializeField] private StageDataList dataList;
    [SerializeField] private StageData stageData;

    [SerializeField] IntReactiveProperty nowWave = new IntReactiveProperty(0);
    [SerializeField] private int maxWave;

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
        nowWave.Subscribe(_ =>
        {
            switch (stageData.waveType[nowWave.Value - 1])
            {
                case StageData.WaveType.Fixed:

                    break;
                case StageData.WaveType.Random:
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
}
