using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
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

    [Header("索敵")]
    public Vector3ReactiveProperty SearchEnemyRP = new Vector3ReactiveProperty();
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

    // inspector拡張
#if UNITY_EDITOR
    [CustomEditor(typeof(HUD_Model))]
    public class HUD_ModelEditor : Editor
    {
        // フラグ等の宣言
        //bool folding = false;
        float fieldSize = 80.0f;

        public override void OnInspectorGUI()
        {
            HUD_Model hm = target as HUD_Model;

            EditorGUILayout.LabelField("体力(現在/最大)");
            EditorGUILayout.BeginHorizontal();
            hm.HealthRP.Value   = EditorGUILayout.IntField(hm.HealthRP.Value, GUILayout.MaxWidth(fieldSize));
            hm.maxHealth        = EditorGUILayout.IntField(hm.maxHealth, GUILayout.MaxWidth(fieldSize));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("バリア(現在/最大)");
            EditorGUILayout.BeginHorizontal();
            hm.BarrierRP.Value  = EditorGUILayout.IntField(hm.BarrierRP.Value, GUILayout.MaxWidth(fieldSize));
            hm.maxBarrier       = EditorGUILayout.IntField(hm.maxBarrier, GUILayout.MaxWidth(fieldSize));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("エネルギー(現在/最大)");
            EditorGUILayout.BeginHorizontal();
            hm.EnergyRP.Value   = EditorGUILayout.IntField(hm.EnergyRP.Value, GUILayout.MaxWidth(fieldSize));
            hm.maxEnergy        = EditorGUILayout.IntField(hm.maxEnergy, GUILayout.MaxWidth(fieldSize));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("アルティメット(現在/最大)");
            EditorGUILayout.BeginHorizontal();
            hm.UltimateRP.Value = EditorGUILayout.IntField(hm.UltimateRP.Value, GUILayout.MaxWidth(fieldSize));
            hm.maxUltimate      = EditorGUILayout.IntField(hm.maxUltimate, GUILayout.MaxWidth(fieldSize));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("スコア/コンボ");
            EditorGUILayout.BeginHorizontal();
            hm.ScoreRP.Value = EditorGUILayout.IntField(hm.ScoreRP.Value, GUILayout.MaxWidth(fieldSize));
            hm.ComboRP.Value = EditorGUILayout.IntField(hm.ComboRP.Value, GUILayout.MaxWidth(fieldSize));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
