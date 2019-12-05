﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ShopBuyController : MonoBehaviour
{
    // 所持マター監視クラス
    // 所持マターを常に監視し、各ショップの項目で購入可能かどうかを
    // チェックするイベントをここに列挙する

    // このスクリプトの動作フラグ
    public BoolReactiveProperty workFlg = new BoolReactiveProperty(false);

    private ShopData buyData;

    // プレイヤー強化のパラメータごとのレベルが格納されているクラス
    [Header("パラメータごとのレベル（設定不要）")]
    private LevelData_Player playerLv;
    // タワー強化のパラメータごとのレベルが格納されているクラス
    private LevelData_Tower[] towerLv;
    // Ult変更のパラメータごとのレベルが格納されているクラス
    private LevelData_Ult ultLv;
    // スキル変更のパラメータごとのレベルが格納されているクラス
    private LevelData_Skill skillLv;

    public Subject<ShopData> BuyControllerInit = new Subject<ShopData>();
    // Start is called before the first frame update
    void Start()
    {
        BuyControllerInit.Subscribe(data => 
        {
            buyData = data;

            playerLv = buyData.levelData_Player;
            skillLv = buyData.levelData_Skill;
            ultLv = buyData.levelData_Ult;
            for (int i = 0; i < buyData.levelData_Tower.Length; i++)
            {
                towerLv[i] = buyData.levelData_Tower[i];
            }
        }).AddTo(this.gameObject);

        ShopManager.Instance.mater
            .Where(mat => buyData != null)
            .Subscribe(mat => 
            {
                // プレイヤー強化購入可能判断処理
                if (mat >= buyData.shopData_Player[playerLv.level_HP.Value + 1].purchaseMater)
                {
                    // 
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Player[playerLv.level_Speed.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Player[playerLv.level_Interval.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                // 赤タワー購入可能判断処理

                if (mat >= buyData.redData_Tower[towerLv[(int)ShopData.TowerColor.Red].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.redData_Tower[towerLv[(int)ShopData.TowerColor.Red].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.redData_Tower[towerLv[(int)ShopData.TowerColor.Red].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.redData_Tower[towerLv[(int)ShopData.TowerColor.Red].level_Repair.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                // 青タワー購入可能判断処理

                if (mat >= buyData.blueData_Tower[towerLv[(int)ShopData.TowerColor.Blue].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.blueData_Tower[towerLv[(int)ShopData.TowerColor.Blue].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.blueData_Tower[towerLv[(int)ShopData.TowerColor.Blue].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.blueData_Tower[towerLv[(int)ShopData.TowerColor.Blue].level_Repair.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                // 黄タワー購入可能判断処理

                if (mat >= buyData.yellowData_Tower[towerLv[(int)ShopData.TowerColor.Yellow].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.yellowData_Tower[towerLv[(int)ShopData.TowerColor.Yellow].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.yellowData_Tower[towerLv[(int)ShopData.TowerColor.Yellow].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.yellowData_Tower[towerLv[(int)ShopData.TowerColor.Yellow].level_Repair.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }


                // 緑タワー購入可能判断処理

                if (mat >= buyData.greenData_Tower[towerLv[(int)ShopData.TowerColor.Green].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.greenData_Tower[towerLv[(int)ShopData.TowerColor.Green].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.greenData_Tower[towerLv[(int)ShopData.TowerColor.Green].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.greenData_Tower[towerLv[(int)ShopData.TowerColor.Green].level_Repair.Value + 1].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                // スキルの購入可能判断処理

                if (mat >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Normal].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }
                if (mat >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Razer].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Missile].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Bomb].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                // 必殺技の購入可能判断処理

                if (mat >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Normal].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Trap].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Bomb].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }

                if (mat >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Repair].purchaseMater)
                {
                    // 購入可能
                }
                else
                {
                    // 金額不足
                }
            }).AddTo(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
