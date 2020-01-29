using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillSword : MonoBehaviour
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

    [SerializeField]
    TDPlayerManager pManager;

    [SerializeField]
    float rotTime = 0.7f;   // 回転速度
    [SerializeField]
    float rotValue = 0.5f;  // 回転量
    [SerializeField]
    float waitSwordGenerate = 0.2f;   // 剣の生成の待ち時間

    // 剣の生成
    public ReactiveProperty<bool> SwordGenerateTrigger = new ReactiveProperty<bool>();
    
    Vector3 startRotation;  // 回転量の初期値

    void Start()
    {
        // 初期設定
        startRotation = this.transform.localEulerAngles;
        bool b;

        // 剣の生成→回転→消滅　イベント
        pManager.SkillTrigger
            .Where(x => pManager.pData.pSkillType == TDPlayerData.SkillTypeList.Sword)
            .Do(_ =>                                        // 剣の生成
            {
                this.transform.localEulerAngles = startRotation;
                SwordGenerateTrigger.Value = true;
            })
            .Delay(TimeSpan.FromSeconds(waitSwordGenerate)) // 生成中の待機時間
            .Subscribe(value =>                             // 剣の回転
            {
            // 剣の回転コルーチンの再生
            Observable.FromCoroutine(_ => RotationCoroutine(rotTime))
            .Delay(System.TimeSpan.FromSeconds(0.1f))// 残心
            .Subscribe(
                    _ => b = true,
                    () => 
                    {
                        SwordGenerateTrigger.Value = false;      // 剣の消滅
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
            rot = Mathf.Lerp(0.0f, 360.0f * rotValue, Mathf.Sin(t / time * 0.5f * Mathf.PI));

            transform.localEulerAngles = startRotation + new Vector3(0, rot, 0);

            t += Time.deltaTime;

            yield return null;
        }
    }
}
