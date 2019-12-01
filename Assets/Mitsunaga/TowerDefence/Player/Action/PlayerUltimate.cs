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

    TDPlayerData pData;

    void Start()
    {
        pData = pManager.pData;

        pManager.ultimateTrigger
            .Where(x => pData.pUltimate.Value >= pData.pMaxUltimate)
            .Subscribe(value =>
            {
                // アルティメットの実行
                Debug.Log("アルティメット　実行");

                pData.pUltimate.Value -= pData.pMaxUltimate;

            }).AddTo(this.gameObject);
    }
}
