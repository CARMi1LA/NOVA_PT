using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDWaveCount : MonoBehaviour
{
    TextMeshProUGUI tmpWaveCount;

    void Start()
    {
        tmpWaveCount = this.GetComponent<TextMeshProUGUI>();
    }
    public void SetWaveCount(int wave,int maxWave)
    {
        tmpWaveCount.text = "WAVE" + wave.ToString() + "/" + maxWave.ToString();
    }
}
