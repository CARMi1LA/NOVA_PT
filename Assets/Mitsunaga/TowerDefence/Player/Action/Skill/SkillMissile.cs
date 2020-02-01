using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillMissile : MonoBehaviour
{
    // ミサイルショット

    [SerializeField]
    TDPlayerManager pManager;

    TDList.BulletTypeList bType = TDList.BulletTypeList.Missile;  // 攻撃タイプの実装テスト

    [SerializeField]
    int missileCount = 3;
    [SerializeField]
    float missileRot = 15.0f;

    void Start()
    {
        pManager.SkillTrigger
            .Where(x => pManager.pData.pSkillType == TDPlayerData.SkillTypeList.Missile)
            .Subscribe(value =>
            {
                for(int i = 0;i < missileCount; ++i)
                {
                    Vector3 angle = Vector3.zero;
                    angle.y += missileRot * i;

                    // スキルの実行
                    TDBulletData bData = new TDBulletData(TDList.ParentList.Player, bType, this.transform.position, this.transform.eulerAngles + angle);
                    TDBulletSpawner.Instance.bulletRentSubject.OnNext(bData);
                }

            }).AddTo(this.gameObject);
    }
}
