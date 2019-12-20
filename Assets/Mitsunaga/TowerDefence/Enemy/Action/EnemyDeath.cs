using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyDeath : MonoBehaviour
{
    // エネミーの死亡処理

    [SerializeField]
    TDEnemyManager eManager;        // エネミーマネージャー

    [SerializeField]
    ParticleSystem eDeathParticle;  // 死亡時のエフェクト

    void Start()
    {
        eManager.DeathTrigger
            .Subscribe(_ =>
            {
                Instantiate(eDeathParticle.gameObject, this.transform.position, Quaternion.identity);

            }).AddTo(this.gameObject);
        
    }
}
