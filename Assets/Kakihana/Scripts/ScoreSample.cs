using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class ScoreSample : MonoBehaviour
{
    // Start is called before the first frame update

    public int highScore;

    // スコアを表示する
    public Text scoreText;
    // ハイスコアを表示する
    public Text highScoreText;

    IntReactiveProperty score;
    public Subject<int> ScoreAdd = new Subject<int>();
    void Start()
    {
        ScoreAdd.Subscribe(point => 
        {
            // ポイントの追加
            score.Value += point;
        }).AddTo(this.gameObject);
        score.Subscribe(_ =>
        {
            // スコアを表示するサンプル
            // スコアがハイスコアより大きければ
            if (highScore < score.Value)
            {
                highScore = score.Value;
            }
            // スコア・ハイスコアを表示する
            scoreText.text = score.ToString();
            highScoreText.text = highScore.ToString();
        }).AddTo(this.gameObject);

        score.Where(_ => highScore < score.Value)
            .Subscribe(_ => 
            {
                // スコアの値が変わって、かつ
                // if (highScore < score.Value)の条件を満たしていたら呼ばれるよ
            }).AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Sample(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                // UnirxでUpdateを使うときはこう記述するよ
                // 0.1秒ごとに処理が実行されるよ
            }).AddTo(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ポイントの追加
    public void AddPoint(int point)
    {
        
    }
}
