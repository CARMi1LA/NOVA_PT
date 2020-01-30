using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTowerHealth : MonoBehaviour
{
    [SerializeField] Renderer[] towerHealthGage;
    [SerializeField] Transform towerTargetObject;
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
    public void SetTowerTarget(MasterData.TowerColor towerColor)
    {
        if(towerColor == MasterData.TowerColor.Blue)
        {
            towerTargetObject.localPosition = towerHealthGage[0].transform.localPosition * 10;
        }
        else if(towerColor == MasterData.TowerColor.Red)
        {
            towerTargetObject.localPosition = towerHealthGage[1].transform.localPosition * 10;
        }
        else if(towerColor == MasterData.TowerColor.Yellow)
        {
            towerTargetObject.localPosition = towerHealthGage[2].transform.localPosition * 10;
        }
        else if(towerColor == MasterData.TowerColor.Green)
        {
            towerTargetObject.localPosition = towerHealthGage[3].transform.localPosition * 10;
        }
    }
}
