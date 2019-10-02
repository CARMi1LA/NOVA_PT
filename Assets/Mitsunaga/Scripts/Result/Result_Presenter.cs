using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Result_Presenter : MonoBehaviour
{
    /*
     * MVPパターン　Presenter

    単体では機能しないModelとViewを繋ぎ、一連の処理を成立させる
    */

    [SerializeField, Header("HUDのModel")]
    HUD_Model hModel;
    [SerializeField, Header("リザルトのModel")]
    Result_Model rModel;

    [SerializeField, Header("リザルトのView")]
    Result_View rView;

    void Start()
    {
        rModel.gamestateRP
            .Where(x => x != 0)
            .Subscribe(value =>
            {

                rView.setResult(value, hModel.ScoreRP.Value, hModel.maxCombo);
            })
            .AddTo(this.gameObject);

        // デバッグ用
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if(Input.GetKeyDown(KeyCode.C))
                    rModel.setState(Result_Model.GAMESTATE.CLEAR);

                if (Input.GetKeyDown(KeyCode.X))
                    rModel.setState(Result_Model.GAMESTATE.GAMEOVER);
            })
            .AddTo(this.gameObject);
    }
}
