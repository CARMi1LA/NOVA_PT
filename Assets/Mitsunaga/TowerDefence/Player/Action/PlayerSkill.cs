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

    public Subject<TDPlayerData.SkillTypeList> StartSkill = new Subject<TDPlayerData.SkillTypeList>();

    void Start()
    {
        pManager.skillTrigger
            .Subscribe(value =>
            {
                // スキルの実行
                Debug.Log("スキル　実行");
                StartSkill.OnNext(value);

            }).AddTo(this.gameObject);
    }
}
