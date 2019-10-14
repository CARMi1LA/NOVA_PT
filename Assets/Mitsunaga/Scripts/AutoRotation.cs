using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AutoRotation : MonoBehaviour
{
    // 一定速度で回転

    [SerializeField]
    Vector3 rotSpeed;

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                this.transform.localEulerAngles += rotSpeed * Time.deltaTime;
            })
            .AddTo(this.gameObject);
    }
}
