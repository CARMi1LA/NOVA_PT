using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerSkill : MonoBehaviour
{
    // スキルマネージャー

    [SerializeField]
    TDPlayerManager pManager;

    TDPlayerData pData;

    void Start()
    {
        pData = pManager.pData;

        pManager.skillTrigger
            .Where(x => pData.pEnergy.Value >= pData.pSkillCost)
            .Subscribe(value =>
            {
                // スキルの実行
                Debug.Log("スキル　実行");

                pData.pEnergy.Value -= pData.pSkillCost;

            }).AddTo(this.gameObject);
    }
}
