using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TMPro;

public class TMP_Flash : MonoBehaviour
{
    // TextMeshProUGUI を点滅させる

    TextMeshProUGUI tmpFlash;

    void Start()
    {
        tmpFlash = GetComponent<TextMeshProUGUI>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                float alpha = Mathf.PingPong(Time.time, 1.0f);

                tmpFlash.color = new Color(tmpFlash.color.r, tmpFlash.color.g, tmpFlash.color.b, alpha);
            })
            .AddTo(this.gameObject);
    }
}
