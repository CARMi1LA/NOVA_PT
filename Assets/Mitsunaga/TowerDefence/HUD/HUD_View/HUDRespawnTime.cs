using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDRespawnTime : MonoBehaviour
{
    // リスポーン時のエフェクト表示

    TextMeshProUGUI rtText;

    void Awake()
    {
        rtText = GetComponent<TextMeshProUGUI>();
    }

    public void SetRespawnTime(bool isDeath)
    {
        rtText.enabled = isDeath;
    }
}
