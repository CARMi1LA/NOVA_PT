using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SwordManager : MonoBehaviour
{
    // 剣による近接攻撃
    /*
    剣の生成
        ShaderのAlpha値の変更
        当たり判定を追加
    剣の回転
        Sinカーブによる回転の実装
    剣の消滅
        当たり判定の削除
        ShaderのAlpha値を変更
    */

    [SerializeField]        // 回転速度
    float rotTime;
    [SerializeField]
    float rotValue = 0.5f;  // 回転量

    public Subject<float> SwordSubject = new Subject<float>();

    // 剣の生成
    public ReactiveProperty<bool> SwordGenerateRP = new ReactiveProperty<bool>();
    [SerializeField] float waitSwordGenerate;   // 剣の生成の待ち時間

    bool isStandby;  // 既に実行しているか
    Vector3 startRotation;  // 回転量の初期値

    void Start()
    {
        // 初期設定
        startRotation = this.transform.localEulerAngles;
        isStandby = true;

        // 剣の生成→回転→消滅　イベント
        SwordSubject
            .Where(x => isStandby)                          // 待機中のみ実行
            .Do(_ =>                                        // 剣の生成
            {
                isStandby = false;
                SwordGenerateRP.Value = true;
            })
            .Delay(TimeSpan.FromSeconds(waitSwordGenerate)) // 生成中の待機時間
            .Subscribe(value =>                             // 剣の回転
            {
            // 剣の回転コルーチンの再生
            Observable.FromCoroutine(_ => RotationCoroutine(value))
            .Subscribe(
                    _ => Debug.Log(""),
                    () => 
                    {
                        SwordGenerateRP.Value = false;      // 剣の消滅
                        isStandby = true;
                    })
                    .AddTo(this.gameObject);
            })
            .AddTo(this.gameObject);
    }

    // 剣の回転コルーチン
    IEnumerator RotationCoroutine(float time)
    {
        float t = 0.0f;
        float rot = 0.0f;

        while (t < time)
        {
            rot = Mathf.Lerp(0.0f, 360.0f, rotValue * Mathf.Sin(t / time * 0.5f * Mathf.PI));

            transform.localEulerAngles = new Vector3(0, rot, 0);

            t += Time.deltaTime;

            yield return null;
        }
    }
}
