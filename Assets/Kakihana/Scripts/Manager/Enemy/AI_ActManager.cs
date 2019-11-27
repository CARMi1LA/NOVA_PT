using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ActManager : MonoBehaviour
{
    // AIステートをもとに敵の移動量などを計算するクラス

    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    public int[] bossAtkList = new int[] {2,3,5,8,9,10};
    public int[] leaderAtkList = new int[] { 11, 12, 13 };

    private void Awake()
    {
        // プレイヤーの座標を取得
        playerTrans = GameManagement.Instance.playerTrans;
    }

    void Start()
    {
        
    }

    // 接近モード処理
    public Vector3 CalcApprMove(Vector3 move, float speed)
    {
        Vector3 dif = playerTrans.position - move;
        float radian = Mathf.Atan2(dif.z, dif.x);
        return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * speed * 10;
    }

    public Vector3 CalcMovePos(Vector3 originPos, float speed)
    {
        return Vector3.zero;
    }

    // プレイヤー間の距離の計算を行うメソッド
    public float CalcDistance(Vector3 enemyPos)
    {
        return (playerTrans.position - enemyPos).sqrMagnitude;
    }

 
}
