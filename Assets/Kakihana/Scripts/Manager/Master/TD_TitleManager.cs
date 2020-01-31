using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class TD_TitleManager : MonoBehaviour
{
    // タワーディフェンス用タイトル
    // 一定速度で回転

    [SerializeField]
    Vector3 rotSpeed;

    // クリックされたか
    public BoolReactiveProperty isClick = new BoolReactiveProperty(false);
    // タイトルエフェクト
    public ParticleSystem titlePS;
    // タイトルCanvas
    public CanvasGroup titleCanvas;
    // フェード用Canvas
    public CanvasGroup fadeCanvas;

    public Subject<Unit> fadeIn = new Subject<Unit>();
    public Subject<Unit> fadeOut = new Subject<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        // フェードイン
        fadeIn
            .Subscribe(_ => 
            {
                fadeCanvas.alpha -= Time.deltaTime;
            }).AddTo(this.gameObject);

        // フェードアウト
        fadeOut
            .Subscribe(_ =>
            {
                fadeCanvas.alpha += Time.deltaTime;
                fadeCanvas.blocksRaycasts = false;
            }).AddTo(this.gameObject);

        // タイトル処理
        this.UpdateAsObservable()
        .Where(_ => isClick.Value == false)
        .Subscribe(_ =>
        {
            // アイドル時はカメラを回転、フェードイン処理
            this.transform.localEulerAngles += rotSpeed * Time.deltaTime;
            if(fadeCanvas.alpha >= 0.01f)
            {
                fadeIn.OnNext(Unit.Default);
            }
            // Aボタン、右クリックでゲームスタート
            if (Input.GetButton("Button_A") == true || Input.GetButtonDown("Fire1") == true)
            {
                isClick.Value = true;
            }
        }).AddTo(this.gameObject);

        // isClickがTrueになったら
        this.UpdateAsObservable()
        .Where(_ => isClick.Value == true)
        .Do(_ => 
        {
            // タイトルのフェードアウト
            if (titleCanvas.alpha >= 0.01f)
            {
                titleCanvas.alpha -= Time.deltaTime;
                titleCanvas.blocksRaycasts = false;
            }
        })
        .Where(_ => titleCanvas.alpha <= 0.01f)
        .Do(_ =>
        {
            // 画面のフェードアウト
            titlePS.Play();
            if (fadeCanvas.alpha < 1)
            {
                fadeOut.OnNext(Unit.Default);
            }
        }).Where(_ => fadeCanvas.alpha >= 1.0f)
        .Sample(System.TimeSpan.FromSeconds(0.5f))
        .Subscribe(_ =>
        {
            // メインシーンへ
            SceneManager.LoadScene("05 StageTD Mitsunaga");
        }).AddTo(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
