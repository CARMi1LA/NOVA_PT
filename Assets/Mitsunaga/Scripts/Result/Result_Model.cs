using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Result_Model : MonoBehaviour
{
    /*
     * MVPパターン　Model
    
    実際に変化、使用するパラメータを格納しておくための場所
    情報だけを置いておき、基本的に処理は書かない
    値が変更された場合、プレゼンターに情報を渡してイベントを発行してもらう
    */

    // リザルトに必要なパラメータ … スコア・最大コンボ・ステージクリアか否か
    // これらのパラメータは、ゲーム全体のマネージャーに保存されていることが多いためそこに合体も

    public enum GAMESTATE
    {
        NULL = 0,
        CLEAR = 1,
        GAMEOVER = 2
    }

    // ゲームの状態
    ReactiveProperty<GAMESTATE> _gamestateRP = new ReactiveProperty<GAMESTATE>();
    public IReadOnlyReactiveProperty<GAMESTATE> gamestateRP
    {
        get { return _gamestateRP; }
    }

    // 各パラメータの初期化
    // ゲーム終了時に何かしらの処理を記述してスコア、コンボ、ゲームの状態を取得する
    // どこに置くべきだろうか … 全体のマネージャーに参照を渡しておき、ゲーム終了時に値を渡してもらう
    public void setState(GAMESTATE gs)
    {
        _gamestateRP.Value = gs;
    }
}
