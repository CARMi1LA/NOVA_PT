using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitManager : MonoBehaviour
{
    // 敵パターンごとの敵数を管理するクラス

    public int unitEnemyCount; // 敵パターンごとの敵数
    // Start is called before the first frame update
    void Start()
    {
        // 子オブジェクトの数を取得
        unitEnemyCount = transform.childCount;
    }
}
