using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerUltimate : MonoBehaviour
{
    // アルティメットマネージャー

    [SerializeField]
    TDPlayerManager pManager;

    // public Subject<TDPlayerData.UltimateTypeList> StartUltimate = new Subject<TDPlayerData.UltimateTypeList>();

    void Start()
    {
        pManager.ultimateTrigger
            .Subscribe(value =>
            {
                // アルティメットの実行
                Debug.Log("アルティメット　実行");

                // StartUltimate.OnNext(value);

            }).AddTo(this.gameObject);
    }
}
