using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AutoMoving : MonoBehaviour
{
    [SerializeField]
    Vector3 moveSpeed;
    [SerializeField]
    bool isPingPong;
    [SerializeField]
    float timeScale;

    void Start()
    {
        Vector3 startPos = transform.position;
        Vector3 movePos;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (isPingPong)
                {
                    movePos = startPos + moveSpeed * Mathf.Sin(Time.time * timeScale);
                }
                else
                {
                    movePos = startPos + moveSpeed * Time.deltaTime;
                }

                transform.position = movePos;
            })
            .AddTo(this.gameObject);
    }
}
