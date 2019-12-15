using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUDUltimate : MonoBehaviour
{
    // アルティメットゲージ表示
    [SerializeField]
    Image ultimateImage;
    [SerializeField]
    float imageMoveSpeed = 3.0f;

    Renderer uRenderer;

    void Start()
    {
        Color uImageColor = ultimateImage.color;
        uRenderer = this.GetComponent<Renderer>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                ultimateImage.color = new Color(uImageColor.r, uImageColor.g, uImageColor.b, Mathf.PingPong(Time.time/imageMoveSpeed, 1));

            }).AddTo(this.gameObject);
    }

    public void SetUltimate(float ultimate, float maxUltimate)
    {
        uRenderer.material.SetFloat("_RingMin", 1 - ultimate / maxUltimate);

    }
}
