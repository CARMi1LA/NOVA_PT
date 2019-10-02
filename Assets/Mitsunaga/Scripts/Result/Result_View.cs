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

    [SerializeField]    // タイムラインの管理
    PlayableDirector    pdResult;
    [SerializeField]    // クリア時のパーティクル
    ParticleSystem      psClear;
    [SerializeField]    // ゲームオーバー時のパーティクル
    ParticleSystem      psGameover;

    [SerializeField]    // タイトルのテキスト
    TextMeshProUGUI     tmpTitle;
    [SerializeField]    // スコアのテキスト
    TextMeshProUGUI     tmpScore;
    [SerializeField]    // コンボのテキスト
    TextMeshProUGUI     tmpCombo;
    [SerializeField]    // ランクのテキスト
    TextMeshProUGUI     tmpRank;


    public void setResult(Result_Model.GAMESTATE state, int score,int combo)
    {
        // スコアとコンボのテキスト変更
        tmpScore.text = score.ToString();
        tmpCombo.text = combo.ToString();

        if (state == Result_Model.GAMESTATE.CLEAR)
        {
            // タイトルをCLEARに変更し、スコアやコンボに応じたクリアランクを表示
            tmpTitle.text = "STAGE CLEAR";
            tmpRank.text = getRank(score, combo);
            // クリア時のパーティクル再生
            psClear.Play();
        }
        else if(state == Result_Model.GAMESTATE.GAMEOVER)
        {
            // タイトルをGAMEOVERに変更し、スコアは0、クリアランクは最低のGを表示
            tmpTitle.text = "GAME OVER";
            tmpRank.text = "G";
            tmpScore.text = "0";
            // ゲームオーバー時のパーティクル再生
            // psGameover.Play();
        }
        tmpRank.materialForRendering.SetColor("_OutlineColor", getRankColor(tmpRank.text));
        pdResult.Play();
    }

    // スコアとコンボの値に応じたクリアランクの文字を返す関数
    string getRank(int score,int combo)
    {
        string rank = "G";

        int rankCount = score / 100 + combo;

        if (rankCount < 100) rank = "C";
        else if (rankCount < 300) rank = "B";
        else if (rankCount < 600) rank = "A";
        else rank = "S";

        return rank;
    }
    // クリアランクの文字に応じた色を返す関数
    Color getRankColor(string rank)
    {
        Color rankColor = new Color(1, 1, 1, 1);

        switch (rank)
        {
            case "S":
                rankColor = new Color(1.0f, 0.4f, 0.7f, 1.0f);
                break;
            case "A":
                rankColor = new Color(1.0f, 0.4f, 0.4f, 1.0f);
                break;
            case "B":
                rankColor = new Color(1.0f, 0.7f, 0.4f, 1.0f);
                break;
            case "C":
                rankColor = new Color(0.4f, 1.0f, 0.7f, 1.0f);
                break;
            default:
                rankColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                break;
        }

        return rankColor;
    }
}
