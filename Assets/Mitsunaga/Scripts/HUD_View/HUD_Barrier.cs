using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUD_Barrier : MonoBehaviour
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

    [SerializeField]
    GameObject[] BarrierImage;

    void Awake()
    {

    }

    public void SetBarrier(int Barrier)
    {
        for(int i = 0; i < BarrierImage.Length; ++i)
        {
            if (i > Barrier - 1)
            {
                BarrierImage[i].SetActive(false);
            }
            else
            {
                BarrierImage[i].SetActive(true);
            }
        }
    }
}
