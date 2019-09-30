using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Playables;
using TMPro;

public class Result_View : MonoBehaviour
{
    /*
     * MVPパターン　View
    
    プレイヤーが目にするものの管理をする、実際にアクションを起こす
    ViewはModelもPresenterも知らない
    */

    [SerializeField]
    PlayableDirector pdResult;

    [SerializeField]
    TextMeshProUGUI scoreTMP;
    [SerializeField]
    TextMeshProUGUI comboTMP;
    [SerializeField]
    TextMeshProUGUI rankTMP;


    public void setResult(int score,int combo)
    {
        scoreTMP.text = score.ToString();
        comboTMP.text = combo.ToString();

        rankTMP.text = "S";
        rankTMP.material.SetColor("_OutlineColor", new Color(1.0f, 0.5f, 0.7f, 1.0f));

        pdResult.Play();
    }
}
