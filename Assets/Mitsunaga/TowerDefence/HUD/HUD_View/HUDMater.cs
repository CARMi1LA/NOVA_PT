using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMater : MonoBehaviour
{
    // 所持マテリアル表示

    TextMeshProUGUI tmpMater;

    void Start()
    {
        tmpMater = this.GetComponent<TextMeshProUGUI>();
    }
    public void SetMater(int mater)
    {
        tmpMater.text = mater.ToString() + " M";
    }
}
