using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TurretManager : MonoBehaviour
{
    // タレットスクリプト
    public enum TurretState
    {
        
    }

    // 親のタワースクリプト
    public TowerManager tower;
    // 敵情報リスト
    public EnemyInfoList enemyInfoList;
    // 防衛認識距離
    [SerializeField] private int turret_RecogDis;
    // 攻撃する標的
    [SerializeField] private Transform targetEnemy;

    // タレットがアクティブ状態であるか
    public BoolReactiveProperty turretActive = new BoolReactiveProperty(false);
    // Start is called before the first frame update
    void Start()
    {
        // タレットが表示されていて標的が設定されなければ動作
        // タワーが生存していなければ動作しない
        this.UpdateAsObservable()
            .Where(_ => tower.towerDeath.Value == false)
            .Where(_ => turretActive.Value == true && targetEnemy == null)
            .Sample(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => 
                {
                    // 敵情報リストより自タレットに一番近い敵を標的に設定
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

        // 標的が設定されている場合の動作
        // タワーが生存していなければ動作しない
        this.UpdateAsObservable()
            .Where(_ => tower.towerDeath.Value == false)
            .Where(_ => turretActive.Value == true && targetEnemy != null)
            .Sample(System.TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ => 
            {
                transform.LookAt(targetEnemy);
                // 弾を発射
                Debug.Log("タレット攻撃");
            }).AddTo(this.gameObject);
    }
}
