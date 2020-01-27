using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBossHealth : MonoBehaviour
{
    Image imgBossHealth;

    void Start()
    {
        imgBossHealth = GetComponent<Image>();
    }
    public void SetBossHealth(int health,int maxHealth)
    {
        imgBossHealth.fillAmount = health / maxHealth;
    }
}
