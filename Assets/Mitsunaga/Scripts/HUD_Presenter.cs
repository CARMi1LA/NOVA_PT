using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUD_Presenter : MonoBehaviour
{
    // Presenter
    /*
    https://qiita.com/nebusokuhibari/items/5e0c36c3b0df78110d32
    PresenterはModelとViewを知っています。
    ModelとViewをつなぐ役割をします。
    コードはModelのイベントにViewの処理を登録しています。

    単体では機能しないModelとViewを繋ぎ、一連の処理を成立させる
    一見ただ冗長にしているだけに見えるが、疎結合化を行うことでスクリプトの拡張性を高めることができると考える
    */

    [SerializeField,Header("プレイヤーのパラメータ")]
    HUD_Model hm;

    [SerializeField,Header("各パラメータの表示UI")]
    HUD_Health hvHealth;
    [SerializeField]
    HUD_Ultimate hvUltimate;
    [SerializeField]
    HUD_Barrier hvBarrier;
    [SerializeField]
    HUD_Energy hvEnergy;
    [SerializeField]
    HUD_Score hvScore;

    private void Start()
    {
        // 各パラメータの初期化
        hm.InitParam();

        // Healthの更新処理
        hm.HealthRP
            .Subscribe(value =>
            {
                hvHealth.SetHealth(hm.maxHealth, value);
            })
            .AddTo(this.gameObject);

        hm.BarrierRP
            .Subscribe(value =>
            {
                hvBarrier.SetBarrier(value);
            })
            .AddTo(this.gameObject);

        hm.EnergyRP
            .Subscribe(value =>
            {
                hvEnergy.SetEnergy(hm.maxEnergy, value);
            })
            .AddTo(this.gameObject);

        hm.UltimateRP
            .Subscribe(value =>
            {
                hvUltimate.SetUltimate(hm.maxUltimate, value);
            })
            .AddTo(this.gameObject);

        hm.ScoreRP
            .Subscribe(value =>
            {
                hvScore.SetScore(value);
            })
            .AddTo(this.gameObject);
    }
}
