using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDEnergy : MonoBehaviour
{
    // エネルギーゲージ表示
    [SerializeField]
    float maxRange = 0.45f;

    Renderer eRenderer;

    void Awake()
    {
        eRenderer = this.GetComponent<Renderer>();
    }

    public void SetEnergy(float energy,float maxEnergy)
    {
        eRenderer.material.SetFloat("_ArcRange", energy / maxEnergy * maxRange);
    }
}
