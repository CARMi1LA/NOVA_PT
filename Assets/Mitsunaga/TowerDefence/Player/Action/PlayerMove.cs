using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 右スティックと左スティックの値を受け取り、Transformの変化量を返す

    Transform playerTransform;

    void Start()
    {
        // 変更を加えるトランスフォームを保存しておく
        // playerTransform = this.transform;
    }

    public float test()
    {
        return Time.time;
    }

    // トランスフォームに変更を加える
    public void ActionMove(InputValueData1P input)
    {
    }
}
