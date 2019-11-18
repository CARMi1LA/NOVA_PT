using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData : MonoBehaviour
{
    public string difficutyName;            // 難易度の名称
    public int playerAttackMag;             // 攻撃力の倍率（プレイヤー）
    public int towerDamageMag;              // 拠点の被ダメージ倍率
    public int towerHPMag;                  // 拠点の耐久力倍率

}
