using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class HUD_SearchEnemy : MonoBehaviour
{
    // View

    [SerializeField, Header("表示するUI")]
    HUD_SearchEnemyMark seMark;
    [SerializeField, Header("表示する距離")]
    float seRange;

    public void SetSearchEnemy(Vector3 playerPos, Vector3 targetPos)
    {
        if(targetPos != Vector3.zero)
        {
            // ターゲットへのベクトルを取得
            Vector3 vec = (targetPos - playerPos).normalized;

            GameObject obj = Instantiate(seMark.gameObject, this.transform);
            obj.transform.localPosition = vec * seRange;
            obj.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
