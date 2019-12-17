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
    
    void Start()
    {
        pManager.ultimateTrigger
            .Subscribe(value =>
            {
                // アルティメット実行時の共通行動(エフェクトなど)

            }).AddTo(this.gameObject);
    }
}
