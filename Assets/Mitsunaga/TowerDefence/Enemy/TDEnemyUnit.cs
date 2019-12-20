using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class TDEnemyUnit : MonoBehaviour
{
    // エネミーのユニットの管理
    [SerializeField]
    TDEnemyManager eManager;

    int eHealth;

    public Subject<int> DeathTrigger = new Subject<int>();

    void Start()
    {
        eManager.unitInitTrigger
            .Subscribe()
    }

}
