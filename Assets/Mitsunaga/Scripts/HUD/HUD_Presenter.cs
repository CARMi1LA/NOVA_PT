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

    [SerializeField,Header("HUDのモデル")]
    HUD_Model hModel;

    [SerializeField,Header("各HUDのビュー")]
    HUD_Health hvHealth;        // 体力
    [SerializeField]
    HUD_Barrier hvBarrier;      // バリア
    [SerializeField]
    HUD_Energy hvEnergy;        // エネルギーゲージ
    [SerializeField]
    HUD_Ultimate hvUltimate;    // アルティメットゲージ
    [SerializeField]
    HUD_Score hvScore;          // スコア
    [SerializeField]
    HUD_Combo hvCombo;          // コンボ
    [SerializeField]
    HUD_Arrow hvArrow;          // ターゲット

    // ターゲット用のプレイヤーの位置とカメラの距離
    [SerializeField]
    Transform playerPosition;
    float cameraDistance = 20.0f;

    private void Start()
    {
        // 各パラメータの初期化
        hModel.InitParam();

        // 各パラメータの更新処理
        hModel.HealthRP
            .Subscribe(value =>
            {
                hvHealth.SetHealth(hModel.maxHealth, value);
            })
            .AddTo(this.gameObject);

        hModel.BarrierRP
            .Subscribe(value =>
            {
                hvBarrier.SetBarrier(value);
            })
            .AddTo(this.gameObject);

        hModel.EnergyRP
            .Subscribe(value =>
            {
                hvEnergy.SetEnergy(hModel.maxEnergy, value);
            })
            .AddTo(this.gameObject);

        hModel.UltimateRP
            .Subscribe(value =>
            {
                hvUltimate.SetUltimate(hModel.maxUltimate, value);
            })
            .AddTo(this.gameObject);

        hModel.ScoreRP
            .Subscribe(value =>
            {
                hvScore.SetScore(value);
            })
            .AddTo(this.gameObject);

        hModel.ComboRP
            .Subscribe(value =>
            {
                hvCombo.SetCombo(value);
            })
            .AddTo(this.gameObject);

        // 更新処理
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                Vector3 mouseScreenPos = Input.mousePosition;
                mouseScreenPos.z = cameraDistance;
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

                // 矢印の更新処理(現在のターゲット > マウスカーソル)
                hvArrow.SetArrow(playerPosition.position, mouseWorldPos);
            })
            .AddTo(this.gameObject);
    }
}
