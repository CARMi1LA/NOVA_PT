using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TowerManager : MonoBehaviour
{
    // 拠点の色、タワー識別用
    private ShopData.TowerColor towerColor;
    private LevelData_Tower towerLv;
    private EnemyInfoList enemyList;

    [SerializeField] private int tower_MaxHP = 100;
    [SerializeField] private IntReactiveProperty towerHp = new IntReactiveProperty(0);
    [SerializeField] private TrapManager[] traps;
    [SerializeField] private TurretManager[] turrets;

    public BoolReactiveProperty towerDeath = new BoolReactiveProperty(false);

    // Start is called before the first frame update
    void Start()
    {
        towerLv = ShopManager.Instance.shopData.levelData_Tower[(int)towerColor];
        towerHp.Value = tower_MaxHP;
        
    }
}
