using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using Random = UnityEngine.Random;
public class EnemyManager : MonoBehaviour,IDamage
{
    // 敵のAI
    public enum EnemyAI
    {
        Approach = 0,       // 接近モード
        Wait,               // 待機モード
        Attack,             // 攻撃モード
        Escape              // 逃走モード
    }

    // 敵データのリスト
    [SerializeField] EnemyDataList enemyDataList;
    // 敵データが格納されているクラス
    [SerializeField] EnemyStatus enemyStatus;
    // AI名称が格納されているクラス
    [SerializeField] AIListManager aiList;
    // 敵の移動量などを計算するクラス
    [SerializeField] AI_ActManager actManager;

    [SerializeField] private int enemyID;
    [SerializeField] IntReactiveProperty enemyHP;
    [SerializeField] EnemyAIReactiveProperty enemyAI = new EnemyAIReactiveProperty();
    [SerializeField] FloatReactiveProperty distance = new FloatReactiveProperty(0.0f);
    // 攻撃可能かどうかを管理するBool型プロパティ
    [SerializeField] BoolReactiveProperty attackFlg = new BoolReactiveProperty(false);
    [SerializeField] private Rigidbody enemyRigid;          // 敵のRigidBody
    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    [SerializeField] private Vector3 movePos;               // 移動ベクトル
    [SerializeField] private float maxDistance = 30.0f;     // プレイヤーとの最大接近距離
    [SerializeField] private float velocityMag = 0.99f;     // 減速倍率

    // 参照用のカスタムプロパティ
    [SerializeField]
    public IReadOnlyReactiveProperty<EnemyAI> enemyAIPropaty
    {
        get { return enemyAI; }
    }

    void Awake()
    {
        enemyDataList = Resources.Load<EnemyDataList>(string.Format("Enemy{0}", enemyID));
        enemyStatus = enemyDataList.EnemyStatusList[GameManagement.Instance.gameLevel.Value - 1];
        playerTrans = GameManagement.Instance.playerTrans;
        aiList = GameManagement.Instance.listManager;
        enemyHP.Value = enemyStatus.hp;
        enemyAI.Value = EnemyAI.Approach;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (enemyStatus.enemyPosition)
        {
            case EnemyStatus.EnemyPosition.Attack:
                AI_NameListAttack AI_Atk = aiList.AI_AtkList;
                AI_Atk.EnemyAIProbSet(enemyStatus.aiLevel);

                enemyAIPropaty.Where(_ => _ == EnemyAI.Approach)
                    .Subscribe(_ =>
                    {
                        int actID = actManager.ChooseAppr(AI_Atk);
                        switch (AI_Atk.AI_Approach[actID])
                        {
                            case "Normal":

                                break;
                            case "Wave":
                                break;
                            case "HighSpeed":
                                break;
                        }
                    }).AddTo(this.gameObject);
                break;
            case EnemyStatus.EnemyPosition.Defence:
                AI_NameListDefence AI_Def = aiList.AI_DefList;
                AI_Def.EnemyAIProbSet(enemyStatus.aiLevel);
                break;
            case EnemyStatus.EnemyPosition.Special:
                break;
            case EnemyStatus.EnemyPosition.Leader:
                AI_NameListLeader AI_Leader = aiList.AI_LeaderList;
                AI_Leader.EnemyAIProbSet(enemyStatus.aiLevel);
                break;
            case EnemyStatus.EnemyPosition.Boss:
                AI_NameListBoss AI_Boss = aiList.AI_BossList;
                AI_Boss.EnemyAIProbSet(enemyStatus.aiLevel);
                break;
            case EnemyStatus.EnemyPosition.Player:
                break;
            default:
                break;
        }

        // 【接近モード移行イベント】
        // 敵が最大接近距離よりも遠ければ接近モードへ移行する
        distance.Where(_ => _ >= Mathf.Pow(maxDistance, 2))
            .Subscribe(_ =>
            {
                velocityMag = 0.99f;
                enemyAI.Value = EnemyAI.Approach;
            }).AddTo(this.gameObject);

        // 【待機モード移行イベント】
        // 敵が最大接近距離に到達したら減速し次の行動を待つ
        distance.Where(_ => _ <= Mathf.Pow(maxDistance, 2))
            .Where(_ => enemyAIPropaty.Value == EnemyAI.Approach)
            .Where(_ => attackFlg.Value == false)
            .Subscribe(_ =>
            {
                velocityMag = 0.66f;
                enemyAI.Value = EnemyAI.Wait;
            }).AddTo(this.gameObject);

        enemyHP.Where(_ => enemyHP.Value <= 0).Subscribe(_ =>
        {
            StageManager.Instance.EnemyDestroy();
        }).AddTo(this.gameObject);

        this.UpdateAsObservable()
        .Sample(TimeSpan.FromSeconds(0.1f))
        .Subscribe(_ =>
        {
            enemyRigid.velocity *= velocityMag;
            distance.Value = (playerTrans.position - this.transform.position).sqrMagnitude;
        }).AddTo(this.gameObject);
    }

    public void HitDamage()
    {
      
    }
}
// 敵AI専用のカスタムプロパティ
[System.Serializable]
public class EnemyAIReactiveProperty : ReactiveProperty<EnemyManager.EnemyAI>
{
    public EnemyAIReactiveProperty() { }
    public EnemyAIReactiveProperty(EnemyManager.EnemyAI initialValue) : base(initialValue) { }
}
