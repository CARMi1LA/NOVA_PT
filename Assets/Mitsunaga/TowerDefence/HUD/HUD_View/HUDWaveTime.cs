using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDWaveTime : MonoBehaviour
{
    // Waveの残り時間表示
    [SerializeField]
    float maxRange = 1.0f;

    Renderer wtRenderer;

    [SerializeField,ColorUsage(false, true)] Color waitColor;
    [SerializeField,ColorUsage(false, true)] Color attackColor;


    void Awake()
    {
        wtRenderer = this.GetComponent<Renderer>();
    }

    public void SetWaveTime(float time,float startTime)
    {
        // 現在タイム / 最大タイム で時計のレンジを設定
        wtRenderer.material.SetFloat("_ArcRange", time / startTime * maxRange);
        // ゲームのフェイズによってカラーを変更
        if (GameManagement.Instance.gameState.Value == GameManagement.BattleMode.Wait)
        {
            wtRenderer.material.SetColor("_MainColor", waitColor);

        }
        else
        {
            wtRenderer.material.SetColor("_MainColor", attackColor);
        }
    }
}
