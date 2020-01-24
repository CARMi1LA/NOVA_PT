using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDWaveTime : MonoBehaviour
{
    // Waveの残り時間表示
    [SerializeField]
    float maxRange = 1.0f;

    Renderer wtRenderer;

    void Awake()
    {
        wtRenderer = this.GetComponent<Renderer>();
    }

    public void SetWaveTime(float time,float startTime)
    {
        wtRenderer.material.SetFloat("_ArcRange", time / startTime * maxRange);
    }
}
