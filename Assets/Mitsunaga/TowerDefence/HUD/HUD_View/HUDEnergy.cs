using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDEnergy : MonoBehaviour
{
    // エネルギーゲージ表示

    Image imgEnergy;

    void Start()
    {
        imgEnergy = this.GetComponent<Image>();
    }

    public void SetEnergy(float energy,float maxEnergy)
    {
        imgEnergy.fillAmount = energy / maxEnergy;
    }
}
