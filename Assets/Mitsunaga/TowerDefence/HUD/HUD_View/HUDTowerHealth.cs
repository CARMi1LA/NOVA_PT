using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTowerHealth : MonoBehaviour
{
    [SerializeField] Renderer[] towerHealthGage;
    [SerializeField] float maxRange = 0.95f;

    void Start()
    {
        
    }

    public void SetTowerHealth(float maxHealth,float[] towerHealth)
    {
        for(int i = 0;i < towerHealthGage.Length; ++i)
        {
            towerHealthGage[i].material.SetFloat("_RingMax", towerHealth[i] / maxHealth * maxRange);
        }
    }
}
