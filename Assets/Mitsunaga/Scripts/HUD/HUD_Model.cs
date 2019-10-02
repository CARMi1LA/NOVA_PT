using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUD_Model : MonoBehaviour
{
    // Model
    /*
    https://qiita.com/nebusokuhibari/items/5e0c36c3b0df78110d32
    Modelはビジネスロジックを記述する部分です。
    UniRxのIntReactivePropertyでプロパティに変更があった際にイベントを実行できるようにします。
    コードのプロパティはプレイヤーの体力値のみです。

    要は実際に変化、使用するパラメータを格納しておくための場所
    情報だけを置いておき、基本的に処理は書かないものと思われる
    ゲームにおいてはプレイヤーのスクリプトに統合されるため、どこかのタイミングで統合予定
    */

    // 各パラメータ
    // 最大値、実数値およびプロパティの宣言

    [Header("体力")]
    public int maxHealth = 10;
    public IntReactiveProperty HealthRP = new IntReactiveProperty();
    [Header("バリア")]
    public int maxBarrier = 3;
    public IntReactiveProperty BarrierRP = new IntReactiveProperty();
    [Header("エネルギー")]
    public int maxEnergy = 100;
    public IntReactiveProperty EnergyRP = new IntReactiveProperty();
    [Header("アルティメット")]
    public int maxUltimate = 100;
    public IntReactiveProperty UltimateRP = new IntReactiveProperty();

    [Header("スコア")]
    public IntReactiveProperty ScoreRP = new IntReactiveProperty();
    [Header("コンボ")]
    public IntReactiveProperty ComboRP = new IntReactiveProperty();

    public int maxCombo = 0;

    // 各パラメータの初期化
    public void InitParam()
    {
        // 最大値に設定
        HealthRP.Value = maxHealth;
        BarrierRP.Value = maxBarrier;
        EnergyRP.Value = maxEnergy;
        // 最小値に設定
        UltimateRP.Value = 0;
        ScoreRP.Value = 0;
        ComboRP.Value = 0;
    }
    // 最大コンボの計測
    void Start()
    {
        ComboRP
            .Subscribe(value =>
            {
                if (value > maxCombo) maxCombo = value;
            })
            .AddTo(this.gameObject);
    }
}
