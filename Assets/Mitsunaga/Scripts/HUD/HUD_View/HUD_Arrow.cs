using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUD_Arrow : MonoBehaviour
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

    public void SetArrow(Vector3 playerPosition , Vector3 targetPosition)
    {
        // ターゲットへの方角を示す矢印

        // ターゲットへのベクトルを取得
        Vector3 vec = (targetPosition - playerPosition).normalized;
        // ベクトルから角度を取得、RadianからDegreeに変換
        float angle = Mathf.Rad2Deg * Mathf.Atan2(vec.z, vec.x);

        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
