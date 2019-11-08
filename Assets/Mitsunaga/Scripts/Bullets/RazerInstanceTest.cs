using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class RazerInstanceTest : MonoBehaviour
{
    // レーザーのシングルトン化の確認および生成の確認

    [SerializeField]
    float waitTime = 1.0f;
    [SerializeField]
    Transform pT;

    float razerStartPosition = 50.0f;

    void Start()
    {
        // デバッグ用
        this.UpdateAsObservable()
            .Sample(System.TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                if (true /*Input.GetKey(KeyCode.R)*/ )
                {
                    // 円内のランダムなポジションをレーザーの始点に設定する
                    Vector3 randomPos = new Vector3(Mathf.Cos(Time.time),Mathf.Sin(Time.time),0) * razerStartPosition;
                    randomPos = pT.position + new Vector3(randomPos.x, 0, randomPos.y);

                    Vector3 randomRot = (pT.position - randomPos).normalized;


                    RazerData rd = new RazerData(RazerData.RazerParent.Enemy, waitTime, 1.0f, randomPos, randomRot);

                    RazerSpawner.Instance.RazerSubject.OnNext(rd);
                }
            })
            .AddTo(this.gameObject);
    }

}
