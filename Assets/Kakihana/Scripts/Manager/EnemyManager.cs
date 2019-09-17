using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyManager : MonoBehaviour,IDamage
{
    // 敵データのリスト
    [SerializeField] EnemyDataList enemyDataList;
    // 敵データが格納されているクラス
    [SerializeField] EnemyStatus enemyStatus;
    [SerializeField] private int enemyID;

    [SerializeField] private Rigidbody myRigid;             // 自分のRigidBody
    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    [SerializeField] private Vector3 movePos;               // 移動ベクトル
    [SerializeField] private float maxDistance = 30.0f;     // プレイヤーとの最大接近距離
    [SerializeField] private float velocityMag = 0.99f;     // 減速倍率

    void Awake()
    {
        enemyDataList = Resources.Load<EnemyDataList>(string.Format("Enemy{0}", enemyID));
        enemyStatus = enemyDataList.EnemyStatusList[GameManagement.Instance.gameLevel.Value - 1];
        playerTrans = GameManagement.Instance.playerTrans;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HitDamage(int hit)
    {

    }
}
