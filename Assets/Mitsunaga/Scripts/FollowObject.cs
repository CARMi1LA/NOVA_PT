using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;
using UniRx.Triggers;

public class FollowObject : MonoBehaviour
{
    [SerializeField]    // ターゲット
    Transform followTarget;
    [SerializeField]    // ターゲットとの距離
    Vector3 followOffset;
    [SerializeField]    // 補間
    bool isLerp;
    [SerializeField]    // 補間の強さ
    float lerpSpeed;

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                Vector3 movePos;
                if (isLerp)
                {
                    movePos = Vector3.Lerp(this.transform.localPosition, followTarget.position + followOffset, lerpSpeed);
                }
                else
                {
                    movePos = followTarget.position + followOffset;
                }

                transform.localPosition = movePos;
            })
            .AddTo(this.gameObject);
    }

// inspector拡張
#if UNITY_EDITOR
    [CustomEditor(typeof(FollowObject))]
    public class FollowObjectEditor : Editor
    {
        bool folding = false;

        public override void OnInspectorGUI()
        {
            FollowObject fObj = target as FollowObject;

            // EditorGUILayout.ObjectField : 任意のコンポーネントの入力
            // 引数について (string ラベル, ターゲットのオブジェクト, typeof(ターゲットの型), bool Scene上のオブジェクトからの参照の可否) as ターゲットの型
            fObj.followTarget   = EditorGUILayout.ObjectField("目標のオブジェクト", fObj.followTarget, typeof(Transform), true) as Transform;
            // EditorGUILayout.Vector3Field : Vector3型の入力
            fObj.followOffset   = EditorGUILayout.Vector3Field("目標との距離", fObj.followOffset);

            // EditorGUILayout.Toggle : bool型の入力
            fObj.isLerp = EditorGUILayout.Toggle("補間", fObj.isLerp);
            // EditorGUILayout.Slider : float型のスライダー(int型はIntSlider)
            // 引数について (入力値, 最小値, 最大値)
            fObj.lerpSpeed = EditorGUILayout.Slider("補間の強さ",fObj.lerpSpeed, 0.0f, 1.0f);
        }
    }
#endif
}
