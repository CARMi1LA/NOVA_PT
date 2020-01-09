using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyInfoList : MonoBehaviour
{
    public List<Transform> enemyInfo = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        // 敵情報リストに追加
        GameManagement.Instance.enemyInfoAdd.Subscribe(val =>
        {
            enemyInfo.Add(val);
        }).AddTo(this.gameObject);

        // １秒毎に敵情報リストのリフレッシュを行う
        this.UpdateAsObservable()
            .Sample(System.TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                foreach (var item in enemyInfo)
                {
                    if (item == null)
                    {
                        enemyInfo.Remove(item);
                    }
                }
            }).AddTo(this.gameObject);
    }
}
