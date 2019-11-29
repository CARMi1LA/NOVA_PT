using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerSkill : MonoBehaviour
{
    // スキル発動イベント
    public Subject<TDPlayerData.SkillTypeList> sTriggerSubject = new Subject<TDPlayerData.SkillTypeList>();

    public void ActionSkill(TDPlayerData pData)
    {
        if(pData.pEnergy.Value >= pData.pSkillCost)
        {
            Debug.Log("スキルを使用");
            sTriggerSubject.OnNext(pData.pSkillType);

            // エネルギーを減少させる
            pData.pEnergy.Value += -pData.pSkillCost;
        }
    }
}
