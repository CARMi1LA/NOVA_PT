using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    // ステージ情報格納クラス

    // Waveごとの敵出現パターン
    public enum WaveType
    {
        Fixed = 0,      // 固定出現、敵パターンオブジェクトに格納されている順に生成
        Random,         // 敵パターンオブジェクトに格納されているものからランダムに選び生成
        Select,         // プレイヤーが敵パターンを任意で選び、生成
        Event,          // イベント発生
        Boss            // ボス生成
    }

    public int stageNo;               // ステージ番号
    public string stageName;          // ステージ名
    private int waveNum;              // ウェーブ数
    public WaveType[] waveType;       // ウェーブごとの敵出現パターン
    public int[] waveTable;           // 出現する敵をここで指定する

    public GameObject[] waveEnemyObj; // 出現する敵パターン
    public GameObject[] eventObj;     // イベント用パターン

    public bool eventFlg;             // イベントが発生するステージかどうか
}
