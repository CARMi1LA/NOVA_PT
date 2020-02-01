using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillBash : MonoBehaviour
{
    // バッシュ、ビッグショット

    [SerializeField]
    TDPlayerManager pManager;

    TDList.BulletTypeList bType = TDList.BulletTypeList.Bash;  // 攻撃タイプの実装テスト

    float bashInterval = 0.3f;

    void Start()
    {
        pManager.SkillTrigger
            .Where(x => pManager.pData.pSkillType == TDPlayerData.SkillTypeList.Bash)
            .Do(_=> 
            {
                // スキルの実行
                TDBulletData bData = new TDBulletData(TDList.ParentList.Player, bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);
            })
            .Delay(System.TimeSpan.FromSeconds(bashInterval))
            .Do(_=> 
            {
                // スキルの実行
                TDBulletData bData = new TDBulletData(TDList.ParentList.Player, bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);
            })
            .Delay(System.TimeSpan.FromSeconds(bashInterval))
            .Subscribe(value =>
            {                
                // スキルの実行
                TDBulletData bData = new TDBulletData(TDList.ParentList.Player, bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);

            }).AddTo(this.gameObject);
    }
}
