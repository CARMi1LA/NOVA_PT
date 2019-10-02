using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    // 揺れの間隔(単位：フレーム)
    int shakeInterval = 5;
    // 初期位置
    Vector3 startPos;

    void Awake()
    {
        startPos = this.transform.position;
        ComboText = this.GetComponent<TextMeshProUGUI>();
    }

    public void SetCombo(int combo)
    {
        ComboText.text = combo.ToString();

        StartCoroutine(ShakeCoroutine(shakePower, shakeTime, shakeInterval));
    }

    // ゆらゆらコルーチン
    IEnumerator ShakeCoroutine(float power, float time, int interval)
    {
        // timeの間、intervalの間隔でpowerの強さで揺らす

        float t = 0.0f;     // 時間の計測
        int count = 0;      // 間隔の計測

        //timeの間ループする
        while (t < time)
        {
            count++;
            if(count % interval == 0)
            {
                transform.position = startPos + GetShake(power);
            }
            
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }

    // ゆらゆらの数値を計算するところ
    Vector3 GetShake(float power)
    {
        // Random.insideUnitCircle：半径1の円の内側からランダムな位置を返す
        return Random.insideUnitCircle * power;
    }
}
