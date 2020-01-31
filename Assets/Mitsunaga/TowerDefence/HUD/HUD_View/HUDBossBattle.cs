using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUDBossBattle : MonoBehaviour
{


    [SerializeField] Image imgBossHealth;
    [SerializeField] GameObject objBossWarning;

    float warningTime = 3.0f;

    void Start()
    {
        imgBossHealth.gameObject.SetActive(false);
        objBossWarning.SetActive(false);
    }
    public void SetBossBattle(TDEnemyUnit boss)
    {
        float maxHealth = Resources.Load<TDEnemyDataList>("TDEnemyDataList").GetEnemyData(TDList.EnemySizeList.Extra, TDList.EnemyTypeList.Attack).eCoreHealth;

        imgBossHealth.gameObject.SetActive(true);
        boss.eHealth
            .Subscribe(value =>
            {
                imgBossHealth.fillAmount = value / maxHealth;

            }).AddTo(this.gameObject);

        objBossWarning.SetActive(true);
        Observable.Timer(System.TimeSpan.FromSeconds(warningTime))
            .Subscribe(_ =>
            {
                objBossWarning.SetActive(false);

            }).AddTo(this.gameObject);
    }
}
