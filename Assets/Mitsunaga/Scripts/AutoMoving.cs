using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;
using UniRx.Triggers;

public class AutoMoving : MonoBehaviour
{
    // 一定速度で移動
    // isPingPongをOnにするとSinカーブで往復する

    [SerializeField]    // 移動量
    Vector3 moveSpeed;
    [SerializeField]    // 往復
    bool isRound;
    [SerializeField]    // 往復の時間
    float timeScale;

    void Start()
    {
        Vector3 startPos = transform.position;
        Vector3 movePos;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (isRound)
                {
                    movePos = startPos + moveSpeed * Mathf.Sin(Time.time * timeScale);
                }
                else
                {
                    movePos = startPos + moveSpeed * Time.deltaTime;
                }

                transform.position = movePos;
            })
            .AddTo(this.gameObject);
    }
    // inspector拡張
#if UNITY_EDITOR
    [CustomEditor(typeof(AutoMoving))]
    public class AutoMovingEditor : Editor
    {
        // フラグ等の宣言
        //bool folding = false;

        public override void OnInspectorGUI()
        {
            AutoMoving am = target as AutoMoving;

            am.moveSpeed = EditorGUILayout.Vector3Field("移動量", am.moveSpeed);
            am.isRound = EditorGUILayout.Toggle("往復", am.isRound);

            if (am.isRound)
            {
                am.timeScale = EditorGUILayout.FloatField("往復時間", am.timeScale);
            }

            // EditorGUILayout.ObjectField : 任意のコンポーネントの入力
            // 引数について (string ラベル, ターゲットのオブジェクト, typeof(ターゲットの型), bool Scene上のオブジェクトからの参照の可否) as ターゲットの型
            //fObj.followTarget = EditorGUILayout.ObjectField("目標のオブジェクト", fObj.followTarget, typeof(Transform), true) as Transform;
            // EditorGUILayout.Vector3Field : Vector3型の入力
            //fObj.followOffset = EditorGUILayout.Vector3Field("目標との距離", fObj.followOffset);

            // EditorGUILayout.Toggle : bool型の入力
            //fObj.isLerp = EditorGUILayout.Toggle("補間", fObj.isLerp);
            // EditorGUILayout.Slider : float型のスライダー(int型はIntSlider)
            // 引数について (入力値, 最小値, 最大値)
            //fObj.lerpSpeed = EditorGUILayout.Slider("補間の強さ", fObj.lerpSpeed, 0.0f, 1.0f);5
        }
    }
#endif
}
