using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using TMPro;

public class TD_GameOverUI : MonoBehaviour
{
    public CanvasGroup mainFade;
    public CanvasGroup clearFade;
    public CanvasGroup gameOverFade;
    public CanvasGroup clearUI;
    public CanvasGroup scoreUI;
    public CanvasGroup gameOverUI;

    public TextMeshProUGUI aliveText;
    public TextMeshProUGUI rankText;

    public Subject<Unit> mainFadeIn = new Subject<Unit>();
    public Subject<Unit> mainFadeOut = new Subject<Unit>();
    public Subject<Unit> clearFadeOut = new Subject<Unit>();
    public Subject<Unit> gameOverFadeOut = new Subject<Unit>();
    public Subject<Unit> gmOverUIFadeOut = new Subject<Unit>();
    public Subject<Unit> clearUIFadeOut = new Subject<Unit>();
    public Subject<Unit> scoreUIFadeOut = new Subject<Unit>();

    public int clearUiCnt;
    // Start is called before the first frame update
    void Start()
    {
        mainFadeIn.Subscribe(_ => 
        {
            mainFade.alpha -= Time.deltaTime;
        }).AddTo(this.gameObject);

        mainFadeOut.Subscribe(_ =>
        {
            mainFade.alpha += Time.deltaTime;
        }).AddTo(this.gameObject);

        clearFadeOut.Subscribe(_ => 
        {
            clearFade.alpha += Time.deltaTime;
        }).AddTo(this.gameObject);

        gameOverFadeOut.Subscribe(_ => 
        {
            gameOverFade.alpha += Time.deltaTime;
        }).AddTo(this.gameObject);

        gmOverUIFadeOut.Subscribe(_ => 
        {
            gameOverUI.alpha += Time.deltaTime;
        }).AddTo(this.gameOverUI);

        clearUIFadeOut.Subscribe(_ => 
        {
            clearUI.alpha += Time.deltaTime;
        }).AddTo(this.gameObject);

        scoreUIFadeOut.Subscribe(_ => 
        {
            scoreUI.alpha += Time.deltaTime;
        }).AddTo(this.gameObject);
    }
}
