using System.Collections;
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

    public SpLvData spLv;
    private ShopData buyData;
    private ShopData.TowerColor twCol;

    public SpBtnPlayerManager spBtnPlayer;
    public SpBtnTowerRManager spBtnTowerR;
    public SpBtnTowerBManager spBtnTowerB;
    public SpBtnTowerYManager spBtnTowerY;
    public SpBtnTowerGManager spBtnTowerG;
    public SpBtnSkillManager spBtnSkill;
    public SpBtnUltManager spBtnUlt;

    public Subject<ShopData> BuyControllerInit = new Subject<ShopData>();
    public Subject<Unit> spMainUpdate = new Subject<Unit>();
    public Subject<Unit> spPlayerUpdate = new Subject<Unit>();
    public Subject<Unit> spRedTwUpdate = new Subject<Unit>();
    public Subject<Unit> spBlueTwUpdate = new Subject<Unit>();
    public Subject<Unit> spYellowTwUpdate = new Subject<Unit>();
    public Subject<Unit> spGreenTwUpdate = new Subject<Unit>();
    public Subject<Unit> spSkillUpdate = new Subject<Unit>();
    public Subject<Unit> spUltUpdate = new Subject<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        BuyControllerInit.Subscribe(data =>
        {

        }).AddTo(this.gameObject);

        spMainUpdate.Subscribe(_ =>
        {
            spPlayerUpdate.OnNext(Unit.Default);
            spRedTwUpdate.OnNext(Unit.Default);
            spBlueTwUpdate.OnNext(Unit.Default);
            spYellowTwUpdate.OnNext(Unit.Default);
            spGreenTwUpdate.OnNext(Unit.Default);
            spSkillUpdate.OnNext(Unit.Default);
            spUltUpdate.OnNext(Unit.Default);
        }).AddTo(this.gameObject);

        // プレイヤー強化購入可能判断処理
        spPlayerUpdate.Subscribe(_ =>
        {
            if (spLv.playerLv.lv_HP.Value >= spLv.playerLv.MAX_LV)
            {
                //spBtnPlayer.SoldOutText.OnNext(ShopData.Player_ParamList.Param_HP);
            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.shopData_Player[spBtnPlayer.nextLvHp].purchaseMater)
                {
                    // 購入可能
                    spBtnPlayer.BuyOkText.OnNext(ShopData.Player_ParamList.Param_HP);
                }
                else
                {
                    Debug.Log("False1");
                    // 購入不可（金額不足）
                    spBtnPlayer.BuyNgText.OnNext(ShopData.Player_ParamList.Param_HP);
                }
            }

            // プレイヤー強化購入可能判断処理
            if (spLv.playerLv.lv_Spd.Value >= spLv.playerLv.MAX_LV)
            {
                //spBtnPlayer.SoldOutText.OnNext(ShopData.Player_ParamList.Param_Speed);
            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.shopData_Player[spLv.playerLv.lv_Spd.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnPlayer.BuyOkText.OnNext(ShopData.Player_ParamList.Param_Speed);
                }
                else
                {
                    // 購入不可（金額不足）
                    spBtnPlayer.BuyNgText.OnNext(ShopData.Player_ParamList.Param_Speed);
                }
            }

            // プレイヤー強化購入可能判断処理
            if (spLv.playerLv.lv_Int.Value >= spLv.playerLv.MAX_LV)
            {
                //spBtnPlayer.SoldOutText.OnNext(ShopData.Player_ParamList.Param_Interval);
            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.shopData_Player[spLv.playerLv.lv_Int.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnPlayer.BuyOkText.OnNext(ShopData.Player_ParamList.Param_Interval);
                }
                else
                {
                    // 購入不可（金額不足）
                    spBtnPlayer.BuyNgText.OnNext(ShopData.Player_ParamList.Param_Interval);
                }
            }
        }).AddTo(this.gameObject);

        // 赤タワー強化購入可能判断処理
        spRedTwUpdate.Subscribe(_ =>
        {
        // トラップ強化
        if (spLv.towerLv[(int)MasterData.TowerColor.Red].level_Trap.Value >= spLv.towerLv[(int)MasterData.TowerColor.Red].MAX_LV)
        {

        }
        else
        {
            if (GameManagement.Instance.mater.Value >= buyData.redData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Red].level_Trap.Value + 1].purchaseMater)
            {
                // 購入可能
                spBtnTowerR.BuyOkText.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
            }
            else
            {
                // 金額不足
                spBtnTowerR.BuyNgText.OnNext(ShopData.TowerRed_ParamList.Param_Trap);
            }
        }

        // タレット強化
        if (spLv.towerLv[(int)MasterData.TowerColor.Red].level_Turret.Value >= spLv.playerLv.MAX_LV)
        {

        }
        else
        {
            if (GameManagement.Instance.mater.Value >= buyData.redData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Red].level_Turret.Value + 1].purchaseMater)
            {
                // 購入可能
                spBtnTowerR.BuyOkText.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
            }
            else
            {
                // 金額不足
                spBtnTowerR.BuyNgText.OnNext(ShopData.TowerRed_ParamList.Param_Turret);
            }
        }

        // タワー強化
        if (spLv.towerLv[(int)MasterData.TowerColor.Red].level_Tower.Value >= spLv.playerLv.MAX_LV)
        {

        }
        else
        {
            if (GameManagement.Instance.mater.Value >= buyData.redData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Red].level_Tower.Value + 1].purchaseMater)
            {
                // 購入可能
                spBtnTowerR.BuyOkText.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
            }
            else
            {
                // 金額不足
                spBtnTowerR.BuyNgText.OnNext(ShopData.TowerRed_ParamList.Param_Tower);
            }
        }

        if (spLv.towerLv[(int)MasterData.TowerColor.Red].level_Repair.Value >= spLv.playerLv.MAX_LV)
        {

        }
        else
        {
                if (GameManagement.Instance.mater.Value >= ShopManager.Instance.repairValue[spLv.towerLv[(int)MasterData.TowerColor.Red].level_Repair.Value + 1])
                {
                    // 購入可能
                    spBtnTowerR.BuyOkText.OnNext(ShopData.TowerRed_ParamList.Repair);
                }
                else
                {
                    // 金額不足
                    spBtnTowerR.BuyNgText.OnNext(ShopData.TowerRed_ParamList.Repair);
                }
            }
        }).AddTo(this.gameObject);

        // 青タワー購入可能判断処理
        spBlueTwUpdate.Subscribe(_ =>
        {
            // トラップ強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Trap.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.blueData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerB.BuyOkText.OnNext(ShopData.TowerBlue_ParamList.Param_Trap);
                }
                else
                {
                    // 金額不足
                    spBtnTowerB.BuyNgText.OnNext(ShopData.TowerBlue_ParamList.Param_Trap);
                }
            }

            // タレット強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Turret.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.blueData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerB.BuyOkText.OnNext(ShopData.TowerBlue_ParamList.Param_Turret);
                }
                else
                {
                    // 金額不足
                    spBtnTowerB.BuyNgText.OnNext(ShopData.TowerBlue_ParamList.Param_Turret);
                }
            }

            // タワー強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Tower.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.blueData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerB.BuyOkText.OnNext(ShopData.TowerBlue_ParamList.Param_Tower);
                }
                else
                {
                    // 金額不足
                    spBtnTowerB.BuyNgText.OnNext(ShopData.TowerBlue_ParamList.Param_Tower);
                }
            }
            // タワー修理
            if (spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Repair.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= ShopManager.Instance.repairValue[spLv.towerLv[(int)MasterData.TowerColor.Blue].level_Repair.Value + 1])
                {
                    // 購入可能
                    spBtnTowerB.BuyOkText.OnNext(ShopData.TowerBlue_ParamList.Repair);
                }
                else
                {
                    // 金額不足
                    spBtnTowerB.BuyNgText.OnNext(ShopData.TowerBlue_ParamList.Repair);
                }
            }
        }).AddTo(this.gameObject);

        // 黄タワー購入可能判断処理
        spYellowTwUpdate.Subscribe(_ =>
        {
            if (spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Trap.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.blueData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerY.BuyOkText.OnNext(ShopData.TowerYellow_ParamList.Param_Trap);
                }
                else
                {
                    // 金額不足
                    spBtnTowerY.BuyNgText.OnNext(ShopData.TowerYellow_ParamList.Param_Trap);
                }
            }

            // タレット強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Turret.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.yellowData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerY.BuyOkText.OnNext(ShopData.TowerYellow_ParamList.Param_Turret);
                }
                else
                {
                    // 金額不足
                    spBtnTowerY.BuyNgText.OnNext(ShopData.TowerYellow_ParamList.Param_Turret);
                }
            }

            // タワー強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Tower.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.yellowData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerY.BuyOkText.OnNext(ShopData.TowerYellow_ParamList.Param_Tower);
                }
                else
                {
                    // 金額不足
                    spBtnTowerY.BuyNgText.OnNext(ShopData.TowerYellow_ParamList.Param_Tower);
                }
            }
            // タワー修理
            if (spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Repair.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= ShopManager.Instance.repairValue[spLv.towerLv[(int)MasterData.TowerColor.Yellow].level_Repair.Value + 1])
                {
                    // 購入可能
                    spBtnTowerY.BuyOkText.OnNext(ShopData.TowerYellow_ParamList.Repair);
                }
                else
                {
                    // 金額不足
                    spBtnTowerY.BuyNgText.OnNext(ShopData.TowerYellow_ParamList.Repair);
                }
            }
       }).AddTo(this.gameObject);

        // 緑タワー購入可能判断処理 
        spGreenTwUpdate.Subscribe(_ =>
        {
            // トラップ強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Green].level_Trap.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.greenData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Green].level_Trap.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerG.BuyOkText.OnNext(ShopData.TowerGreen_ParamList.Param_Trap);
                }
                else
                {
                    // 金額不足
                    spBtnTowerG.BuyNgText.OnNext(ShopData.TowerGreen_ParamList.Param_Trap);
                }
            }

            // タレット強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Green].level_Turret.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.greenData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Green].level_Turret.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerG.BuyOkText.OnNext(ShopData.TowerGreen_ParamList.Param_Turret);
                }
                else
                {
                    // 金額不足
                    spBtnTowerG.BuyNgText.OnNext(ShopData.TowerGreen_ParamList.Param_Turret);
                }
            }

            // タワー強化
            if (spLv.towerLv[(int)MasterData.TowerColor.Green].level_Tower.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= buyData.greenData_Tower[spLv.towerLv[(int)MasterData.TowerColor.Green].level_Tower.Value + 1].purchaseMater)
                {
                    // 購入可能
                    spBtnTowerG.BuyOkText.OnNext(ShopData.TowerGreen_ParamList.Param_Tower);
                }
                else
                {
                    // 金額不足
                    spBtnTowerG.BuyNgText.OnNext(ShopData.TowerGreen_ParamList.Param_Tower);
                }
            }
            // タワー修理
            if (spLv.towerLv[(int)MasterData.TowerColor.Green].level_Repair.Value >= spLv.playerLv.MAX_LV)
            {

            }
            else
            {
                if (GameManagement.Instance.mater.Value >= ShopManager.Instance.repairValue[spLv.towerLv[(int)MasterData.TowerColor.Green].level_Repair.Value + 1])
                {
                    // 購入可能
                    spBtnTowerG.BuyOkText.OnNext(ShopData.TowerGreen_ParamList.Repair);
                }
                else
                {
                    // 金額不足
                    spBtnTowerG.BuyNgText.OnNext(ShopData.TowerGreen_ParamList.Repair);
                }
            }
        }).AddTo(this.gameObject);

        // スキル購入可否処理
        spSkillUpdate.Subscribe(_ =>
        {
            if (GameManagement.Instance.mater.Value >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Normal].purchaseMater && spBtnSkill.normal.Value == false)
            {
                spBtnSkill.BuyOkText.OnNext(ShopData.Skill_ParamList.Normal);
            }
            else
            {
                spBtnSkill.BuyNgText.OnNext(ShopData.Skill_ParamList.Normal);
            }

            if (GameManagement.Instance.mater.Value >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Razer].purchaseMater && spBtnSkill.razer.Value == false)
            {
                spBtnSkill.BuyOkText.OnNext(ShopData.Skill_ParamList.Razer);
            }
            else
            {
                spBtnSkill.BuyNgText.OnNext(ShopData.Skill_ParamList.Razer);
            }

            if (GameManagement.Instance.mater.Value >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Missile].purchaseMater && spBtnSkill.missile.Value == false)
            {
                spBtnSkill.BuyOkText.OnNext(ShopData.Skill_ParamList.Missile);
            }
            else
            {
                spBtnSkill.BuyNgText.OnNext(ShopData.Skill_ParamList.Missile);
            }

            if (GameManagement.Instance.mater.Value >= buyData.shopData_Skill[(int)ShopData.Skill_ParamList.Bomb].purchaseMater && spBtnSkill.bomb.Value == false)
            {
                spBtnSkill.BuyOkText.OnNext(ShopData.Skill_ParamList.Bomb);
            }
            else
            {
                spBtnSkill.BuyNgText.OnNext(ShopData.Skill_ParamList.Bomb);
            }
        }).AddTo(this.gameObject);

        // 必殺技購入可否処理
        spUltUpdate.Subscribe(_ =>
        {
            if (GameManagement.Instance.mater.Value >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Normal].purchaseMater && spBtnUlt.normal.Value == false)
            {
                spBtnUlt.BuyOkText.OnNext(ShopData.Ult_ParamList.Normal);
            }
            else
            {
                spBtnUlt.BuyNgText.OnNext(ShopData.Ult_ParamList.Normal);
            }

            if (GameManagement.Instance.mater.Value >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Trap].purchaseMater && spBtnUlt.trap.Value == false)
            {
                spBtnUlt.BuyOkText.OnNext(ShopData.Ult_ParamList.Trap);
            }
            else
            {
                spBtnUlt.BuyNgText.OnNext(ShopData.Ult_ParamList.Trap);
            }

            if (GameManagement.Instance.mater.Value >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Bomb].purchaseMater && spBtnUlt.bomb.Value == false)
            {
                spBtnUlt.BuyOkText.OnNext(ShopData.Ult_ParamList.Bomb);
            }
            else
            {
                spBtnUlt.BuyNgText.OnNext(ShopData.Ult_ParamList.Bomb);
            }

            if (GameManagement.Instance.mater.Value >= buyData.shopData_Ult[(int)ShopData.Ult_ParamList.Repair].purchaseMater && spBtnUlt.repair.Value == false)
            {
                spBtnUlt.BuyOkText.OnNext(ShopData.Ult_ParamList.Repair);
            }
            else
            {
                spBtnUlt.BuyNgText.OnNext(ShopData.Ult_ParamList.Repair);
            }
        }).AddTo(this.gameObject);
        GameManagement.Instance.mater
            .Where(mat => buyData != null && spLv != null)
            .Subscribe(mat =>
            {
                spMainUpdate.OnNext(Unit.Default);
            }).AddTo(this.gameObject);
    }
    public void BuyInit()
    {
        buyData = ShopManager.Instance.shopData;
        spLv = ShopManager.Instance.spLv;
        Debug.Log("BuyControllInit");
    }
}