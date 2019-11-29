using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    // プレイヤーの死亡演出

    [SerializeField] ParticleSystem psDeath;    // エフェクト

    void Start()
    {
        
    }

    // 死亡演出
    public void ActionDeath()
    {
        // エフェクト再生
        psDeath.Play();
    }
}
