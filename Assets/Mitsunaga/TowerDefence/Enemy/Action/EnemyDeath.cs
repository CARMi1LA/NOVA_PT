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
                Instantiate(eUnit.eManager.deathParticle.gameObject, this.transform.position, Quaternion.identity);
                Destroy(eUnit.gameObject);

                new ItemData(eUnit.eManager.eData.eDropMater, 0, 0, ItemManager.ItemType.Mater, this.transform.position);


            }).AddTo(this.gameObject);
        
    }
}
