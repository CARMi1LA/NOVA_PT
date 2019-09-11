using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    // ステージ情報格納クラス

    public int stageNo;     // ステージ番号
    public int waveNum;     // ウェーブ数
    public int[] waveTable;   // ウェーブごとの敵出現パターン

    public GameObject[] waveEnemyObj;
    public GameObject[] eventObj;

    public bool eventFlg;

    // Start is called before the first frame update
    void Start()
    {
        waveNum = waveTable.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
