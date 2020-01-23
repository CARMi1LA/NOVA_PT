using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyDeath : MonoBehaviour
{
    // ユニットの死亡処理

    [SerializeField]
    TDEnemyUnit eUnit;

    void Start()
    {
        eUnit.DeathTrigger
            .Subscribe(_ =>
            {
                Debug.Log("死んだ");
                Instantiate(eUnit.eManager.deathParticle.gameObject, this.transform.position, Quaternion.identity);
                Destroy(eUnit.gameObject);

            }).AddTo(this.gameObject);
        
    }
}
