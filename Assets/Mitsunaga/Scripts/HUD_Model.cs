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

    // Health
    public int maxHealth = 10;
    public IntReactiveProperty HealthRP = new IntReactiveProperty();
    // Barrier
    public int maxBarrier = 3;
    public IntReactiveProperty BarrierRP = new IntReactiveProperty();
    // Energy
    public int maxEnergy = 100;
    public IntReactiveProperty EnergyRP = new IntReactiveProperty();
    // Ultimate
    public int maxUltimate = 100;
    public IntReactiveProperty UltimateRP = new IntReactiveProperty();

    // Score
    public IntReactiveProperty ScoreRP = new IntReactiveProperty();
}
