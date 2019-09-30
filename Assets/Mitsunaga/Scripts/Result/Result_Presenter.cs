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

    [SerializeField, Header("リザルトのModel")]
    Result_Model rModel;

    [SerializeField, Header("リザルトのView")]
    Result_View rView;

    void Start()
    {
        rModel.gamestateRP
            .Where(x => x != 0)
            .Subscribe(_ =>
            {
                rView.setResult(rModel.score, rModel.combo);
            })
            .AddTo(this.gameObject);

        // デバッグ用
        this.UpdateAsObservable()
            .Where(x => Input.GetKeyDown(KeyCode.C))
            .Subscribe(_ =>
            {
                rModel.setResult(Result_Model.GAMESTATE.CLEAR, 81237894, 1245);
            })
            .AddTo(this.gameObject);
    }
}
