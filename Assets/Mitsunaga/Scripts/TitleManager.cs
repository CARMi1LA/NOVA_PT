using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // あとでMVPパターンで作り直します

    [SerializeField,Header("タイトル画面のタイムライン")]
    PlayableDirector pbCanvas;  // タイムライン管理
    CanvasGroup cgCanvas;       // 当たり判定をコントロールするキャンバスグループ

    // ステージセレクトでのパネル遷移用変数
    [SerializeField]
    RectTransform pObject;
    int pRange      = 3;        // パネルの数
    int pSize       = 1920;     // パネルのサイズ
    float pSpeed    = 0.1f;     // パネル移動のスピード
    float pError    = 0.005f;   // パネル移動の誤差
    
    IntReactiveProperty pCountRP = new IntReactiveProperty(0);

    void Start()
    {
        cgCanvas = pbCanvas.gameObject.GetComponent<CanvasGroup>();

        // パネル遷移を実行
        pCountRP
            .Subscribe(value =>
            {
                StopAllCoroutines();
                StartCoroutine(pChangeCoroutine(value));
            })
            .AddTo(this.gameObject);

        // 最初のクリック時のみ、ステージセレクトへのタイムラインを実行
        this.UpdateAsObservable()
            .First(x => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                pbCanvas.Play();
            })
            .AddTo(this.gameObject);
    }

    // パネル遷移のコルーチン
    IEnumerator pChangeCoroutine(int pID)
    {
        Vector3 movePos = pObject.localPosition;
        float targetX = pID * -pSize;

        // 誤差の範囲内になるまで移動
        while (Mathf.Abs(1 - (movePos.x / targetX)) >= pError)
        {
            movePos.x = Mathf.Lerp(movePos.x, targetX, pSpeed);

            pObject.localPosition = movePos;
            yield return null;
        }
        // 位置を揃える
        movePos.x = targetX;
        pObject.localPosition = movePos;
    }

    // シーン遷移のコルーチン
    IEnumerator GameStartCoroutine(float waitTime)
    {
        // ボタンなどのクリックを不可能にする
        cgCanvas.blocksRaycasts = false;
        // 指定時間待つ
        float time = 0.0f;

        while (time < waitTime)
        {
            time += Time.deltaTime;

            yield return null;
        }

        // シーン遷移
        SceneManager.LoadScene(0);
    }

    // ボタン処理
    public void OnLeftButtonClicked() // 左矢印ボタン
    {
        if(pCountRP.Value > 0)
        {
            pCountRP.Value--;
        }
    }
    public void OnRightButtonClicked() // 右矢印ボタン
    {
        if (pCountRP.Value < pRange - 1)
        {
            pCountRP.Value++;
        }
    }
    public void OnStartButtonClicked() // ゲームスタートボタン
    {
        StartCoroutine(GameStartCoroutine(0.5f));
    }
}
