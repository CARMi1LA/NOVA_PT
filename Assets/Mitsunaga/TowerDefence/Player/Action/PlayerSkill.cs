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

    void Start()
    {
        pManager.skillTrigger
            .Where(x => pManager.pData.pEnergy.Value >= pManager.pData.pSkillCost)
            .Subscribe(value =>
            {
                // スキルの実行
                Debug.Log("スキル　実行");

            }).AddTo(this.gameObject);
    }
}
