using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyGoToHorizon : MonoBehaviour
{
    float PosY = 0.0f;
    float LerpPower = 0.02f;

    void Start()
    {
        this.UpdateAsObservable()
            .Do(_ =>
            {
                this.transform.position = new Vector3(
                    this.transform.position.x,
                    Mathf.Lerp(this.transform.position.y, PosY, LerpPower),
                    this.transform.position.z
                    );
            })
            .Where(x => Mathf.Abs(this.transform.position.y) - PosY <= 0.01f)
            .Subscribe(_ =>
            {
                this.transform.position = new Vector3(
                    this.transform.position.x,
                    PosY,
                    this.transform.position.z
                    );
            }).AddTo(this.gameObject);
            
    }
}
