using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UniRx;
using UniRx.Triggers;
using TMPro;

public class HUD_Combo : MonoBehaviour
{
    // View
    /*
    https://qiita.com/nebusokuhibari/items/5e0c36c3b0df78110d32
    MVPのViewはModelもPresenterも知らないので、
    これらを宣言しないようにしてください。
    コードは体力Barの赤い部分を変更する処理で、これはイベントに登録するメソッドになります。

    実際にプレイヤーに見える部分に変更する処理を
    UIにおいてはHPバーの大きさを変更したり、スコアの数値を反映させるなど
    
    もしかしてパラメータごとにViewのスクリプトは分けたほうが良い？そんな気がしてきた
    */

    TextMeshProUGUI ComboText;

    [SerializeField, Header("揺れの強さ")]
    float shakePower;
    [SerializeField, Header("揺れの長さ")]
    float shakeTime;
    [SerializeField, Header("揺れの間隔(フレーム)")]
    int shakeInterval = 2;
    [SerializeField, Header("揺れ時の色変化")]
    Color shakeColor = new Color(1, 1, 1, 1);
    // 初期位置
    Vector3 startPos;
    float   startFontSize;
    Color   startFontColor;

    void Awake()
    {
        // 初期化
        ComboText       = this.GetComponent<TextMeshProUGUI>();
        startFontSize   = ComboText.fontSize;
        startFontColor  = ComboText.color;
        startPos        = this.transform.localPosition;
    }

    public void SetCombo(int combo)
    {
        ComboText.text = combo.ToString();

        StartCoroutine(ShakeCoroutine(shakePower, shakeTime, shakeInterval));
    }

    // ゆらゆらコルーチン
    IEnumerator ShakeCoroutine(float power, float time, int interval)
    {
        // timeの間、intervalの間隔でpowerの強さで揺らす 文字も大きくする 色も変える

        // Cosカーブを用いて変化量をだんだん減らす
        // Mathf.Cos(90 * (t / time) * Mathf.Deg2Rad) tが0のとき1　tが1のとき0　をCosカーブで補間

        float t = 0.0f;     // 時間の計測
        int count = 0;      // 間隔の計測

        //timeの間ループする
        while (t < time)
        {
            count++;
            if(count % interval == 0)
            {
                // Random.onUnitSphere：半径1の球体からランダムな表面の位置を返す
                transform.localPosition = startPos + (Random.onUnitSphere * (power * Mathf.Cos(90 * (t / time) * Mathf.Deg2Rad)));
            }
            ComboText.fontSize  = startFontSize + (startFontSize * 0.5f * Mathf.Cos(90 * (t / time) * Mathf.Deg2Rad));
            ComboText.color     = Color.Lerp(shakeColor, startFontColor, t / time);

            t += Time.deltaTime;
            yield return null;
        }

        // 初期値に調整する
        ComboText.fontSize      = startFontSize;
        ComboText.color         = startFontColor;
        transform.localPosition = startPos;
    }

    // inspector拡張
#if UNITY_EDITOR
    [CustomEditor(typeof(HUD_Combo))]
    public class HUD_ComboEditor : Editor
    {
        // フラグ等の宣言

        public override void OnInspectorGUI()
        {
            HUD_Combo hc = target as HUD_Combo;

            hc.shakePower       = EditorGUILayout.FloatField("揺れの強さ", hc.shakePower);
            hc.shakeTime        = EditorGUILayout.FloatField("揺れの長さ", hc.shakeTime);
            hc.shakeInterval    = EditorGUILayout.IntField  ("揺れの間隔", hc.shakeInterval);
            hc.shakeColor       = EditorGUILayout.ColorField("揺れ時の色変更", hc.shakeColor);
        }
    }
#endif
}
