using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUltimate : MonoBehaviour
{
    // アルティメットゲージ表示

    Image imgUltimate;

    void Start()
    {
        imgUltimate = this.GetComponent<Image>();
    }

    public void SetUltimate(float ultimate, float maxUltimate)
    {
        imgUltimate.fillAmount = ultimate / maxUltimate;
    }
}
