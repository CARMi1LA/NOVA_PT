using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerSkill : MonoBehaviour
{

    // スキルでの共通行動

    [SerializeField]
    TDPlayerManager pManager;

    void Start()
    {
        pManager.skillTrigger
            .Subscribe(value =>
            {
                // スキルの実行
                Debug.Log("スキル　実行" + value.ToString());
                // スキル発動時の共通行動などを記録しておく

            }).AddTo(this.gameObject);
    }
}
