using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopTowerPramData
{
    // タワーのパラメータを格納するクラス
    /*  軽量化のため各防衛施設のデータはこちらでは設定せず個別で行い、
        レベルごとのインデックスだけを持つ 
    */
    public int trapParamIndex;      // トラップデータのインデックス
    public int turretParamIndex;    // タレットデータのインデックス
    public int towerParamIndex;     // タワーデータのインデックス
    public int repairCount;         // 修理回数
    public int purchaseMater;       // 必要金額
}
