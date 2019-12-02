using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealth : MonoBehaviour
{
    // ヘルスゲージ表示

    Image imgHealth;

    void Start()
    {
        imgHealth = this.GetComponent<Image>();
    }

    public void SetHealth(int health,int maxHealth)
    {
        imgHealth.fillAmount = health / maxHealth;
    }
}
