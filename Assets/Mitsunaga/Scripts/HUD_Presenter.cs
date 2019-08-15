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

    HUD_Model hm;

    [SerializeField]
    HUD_Health hvHealth;

    private void Awake()
    {
        // Healthの更新処理
        hm.HealthRP
            .Subscribe(value =>
            {
                hvHealth.SetHealth(hm.maxHealth, value);
            })
            .AddTo(this.gameObject);
    }
}
