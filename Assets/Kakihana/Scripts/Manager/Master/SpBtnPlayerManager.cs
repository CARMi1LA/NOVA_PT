using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpBtnPlayerManager : MonoBehaviour
{
    public LevelData_Player levelData_Player;

    Subject<Unit> InitSubject = new Subject<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        InitSubject.Subscribe(_ => 
        {
            levelData_Player = ShopManager.Instance.shopData.levelData_Player;
        }).AddTo(this.gameObject);

        levelData_Player.level_HP.Subscribe(_ =>
        {
            //HPボタンUIの処理
        }).AddTo(this.gameObject);

        levelData_Player.level_Speed.Subscribe(_ =>
        {
            //速度ボタンUIの処理
        }).AddTo(this.gameObject);

        levelData_Player.level_Interval.Subscribe(_ =>
        {
            //発射間隔ボタンUIの処理
        }).AddTo(this.gameObject);
    }
}
