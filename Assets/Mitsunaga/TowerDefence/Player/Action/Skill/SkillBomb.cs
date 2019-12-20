using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillBomb : MonoBehaviour
{
    // 通常攻撃

    [SerializeField]
    TDPlayerManager pManager;
    [SerializeField]
    TDList.BulletTypeList bType;  // 攻撃タイプの実装テスト
    void Start()
    {
        pManager.skillTrigger
            .Where(x => pManager.pData.pSkillType == TDPlayerData.SkillTypeList.Bomb)
            .Subscribe(value =>
            {                
                // 通常攻撃の実行
                TDBulletData bData = new TDBulletData(TDList.ParentList.Enemy, bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);

            }).AddTo(this.gameObject);
    }
}
