using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillBomb : MonoBehaviour
{
    // 通常攻撃

    [SerializeField]
    PlayerSkill pSkill;
    [SerializeField]
    TDBulletData.BulletTypeList bType;  // 攻撃タイプの実装テスト
    void Start()
    {
        pSkill.SelectSkillTrigger
            .Subscribe(value =>
            {                
                // 通常攻撃の実行
                Debug.Log("通常攻撃　実行");
                TDBulletData bData = new TDBulletData(TDBulletData.BulletParentList.Enemy, bType, this.transform.position, this.transform.eulerAngles);
                TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);

            }).AddTo(this.gameObject);
    }
}
