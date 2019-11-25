using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TowerManager : MonoBehaviour
{
    // 拠点の色（仮）
    public enum TowerColor
    {
        Red,
        Yellow,
        Blue,
        Green
    }

    public TowerColor towerColor;

    [SerializeField] private int tower_MaxHP = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
