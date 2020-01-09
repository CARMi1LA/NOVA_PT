using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TurretManager : MonoBehaviour
{
    // タレットスクリプト

    public TowerManager tower;
    public EnemyInfoList enemyInfoList;

    // 防衛認識距離
    [SerializeField] private int turret_RecogDis;
    // 攻撃する標的
    [SerializeField] private Transform targetEnemy;

    public BoolReactiveProperty turretActive = new BoolReactiveProperty(false);
    // Start is called before the first frame update
    void Start()
    {
        turretActive.Where(_ => turretActive.Value == false)
            .Subscribe(_ =>
            {
                Color aplha = gameObject.GetComponent<Material>().color;
                aplha.a = 0.0f;
                this.GetComponent<Renderer>().material.color = aplha;
            }).AddTo(this.gameObject);

        turretActive.Where(_ => turretActive.Value == true)
            .Subscribe(_ => 
            {
                Color aplha = gameObject.GetComponent<Material>().color;
                aplha.a = 1.0f;
                this.GetComponent<Renderer>().material.color = aplha;
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => turretActive.Value == true && targetEnemy == null)
            .Sample(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => 
                {
                    var dis = 0.0f;
                    foreach (var item in enemyInfoList.enemyInfo)
                    {
                        dis = (this.transform.position - item.position).sqrMagnitude;
                        if (dis <= Mathf.Pow(turret_RecogDis, 2))
                        {
                            targetEnemy = item;
                        }
                    }
                }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(_ => turretActive.Value == true && targetEnemy != null)
            .Sample(System.TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => 
            {

            }).AddTo(this.gameObject);
    }
}
