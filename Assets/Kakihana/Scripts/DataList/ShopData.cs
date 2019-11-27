using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    public int[,] level_Player;     // プレイヤーの各パラメータレベル（パラメーター名,レベル）
    public int[,,] level_Tower;     // タワーの各パラメータレベル（タワー番号,パラメーター名,レベル）
    public int[] level_Ult;        // スキル＆必殺技の各パラメータ

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
