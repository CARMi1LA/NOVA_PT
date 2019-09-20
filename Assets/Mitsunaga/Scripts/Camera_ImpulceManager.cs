using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cinemachine;

public class Camera_ImpulceManager : MonoBehaviour
{
    // Cinemachineを利用して、画面の揺れを実装する
    CinemachineImpulseSource CISource;

    [SerializeField,Header("衝撃の大きさ")]
    Vector3 impulsePower;

    // 揺れを発生させるストリーム
    public BoolReactiveProperty impulseRP = new BoolReactiveProperty(false);

    void Awake()
    {
        CISource = this.GetComponent<CinemachineImpulseSource>();
    }
    void Start()
    {
        // 
        impulseRP
            .Where(x => x)
            .Subscribe(_ =>
            {
                CISource.GenerateImpulse(impulsePower);
                impulseRP.Value = false;
            })
            .AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(x => Input.GetKeyDown(KeyCode.I))
            .Subscribe(_ =>
            {
                impulseRP.Value = true;
            })
            .AddTo(this.gameObject);
    }
}
