using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameInputManager : MonoBehaviour
{
    // キー入力量を取得するクラス

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => 
            {

            }).AddTo(this.gameObject);
    }
}
