using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealth : MonoBehaviour
{
    // ヘルスゲージ表示
    [SerializeField]
    float maxRange = 0.45f;
    
    Renderer hRenderer;

    void Start()
    {
        hRenderer = this.GetComponent<Renderer>();
    }

    public void SetHealth(float health,float maxHealth)
    {
        hRenderer.material.SetFloat("_ArcRange", health / maxHealth * maxRange);
    }
}