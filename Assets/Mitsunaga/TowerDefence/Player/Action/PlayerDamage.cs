using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // プレイヤーのダメージ演出

    [SerializeField] ParticleSystem psDamage;   // エフェクト

    void Start()
    {
        
    }

    // ダメージ演出
    public void ActionDamage()
    {
        // エフェクト再生
        psDamage.Play();

        // ここ適当
        Debug.Log("ダメージ計算などを行おうと思ったが計算するところが見当たらないんですが");
    }
}
