using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUD_Health : MonoBehaviour
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

    float maxFillAmount = 0.77f;
    Image HealthImage;

    void Awake()
    {
        HealthImage = this.GetComponent<Image>();
    }

    public void SetHealth(int maxHealth, int Health)
    {
        HealthImage.fillAmount = maxFillAmount * ((float)Health / maxHealth);
    }
}
