using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUDBossBattle : MonoBehaviour
{
    Image imgBossHealth;

    void Start()
    {
        imgBossHealth = GetComponent<Image>();
    }
    public void SetBossBattle(TDEnemyUnit boss)
    {
        float maxHealth = boss.eManager.eData.eCoreHealth;

        boss.eHealth
            .Subscribe(value =>
            {
                imgBossHealth.fillAmount = boss.eHealth.Value / maxHealth;

            }).AddTo(this.gameObject);
    }
}
