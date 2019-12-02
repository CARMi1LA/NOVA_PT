using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerUltimate : MonoBehaviour
{
    // スキルマネージャー

    [SerializeField]
    TDPlayerManager pManager;

    void Start()
    {
        pManager.ultimateTrigger
            .Subscribe(value =>
            {
                // アルティメットの実行
                Debug.Log("アルティメット　実行");

            }).AddTo(this.gameObject);
    }
}
