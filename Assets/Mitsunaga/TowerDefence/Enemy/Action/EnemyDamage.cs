using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyDamage : MonoBehaviour
{
    // ダメージ処理

    [SerializeField]
    TDEnemyUnit eUnit;

    Renderer damageRenderer;
    float damageTime = 1.0f;
    Subject<float> DamageTrigger = new Subject<float>();
    void Awake()
    {
        damageRenderer = this.GetComponent<Renderer>();   
    }
    void Start()
    {
        eUnit.DamageTrigger
        .Subscribe(_ =>
        {
            // ダメージをヘルスに適用、キャラを点滅、プレイヤー狙いに変更
            eUnit.eHealth.Value--;
            eUnit.eManager.isTargetPlayer.Value = true;
            Debug.Log("ターゲットを変更");
            DamageTrigger.OnNext(damageTime);

        }).AddTo(this.gameObject);
        // 一定時間、マテリアルを点滅させる
        DamageTrigger
            .Do(value =>
            {
                damageRenderer.material.SetInt("_IsDamage", 1);
            })
            .Delay(System.TimeSpan.FromSeconds(damageTime))
            .Subscribe(value =>
            {
                damageRenderer.material.SetInt("_IsDamage", 0);

            }).AddTo(this.gameObject);


    }
}
