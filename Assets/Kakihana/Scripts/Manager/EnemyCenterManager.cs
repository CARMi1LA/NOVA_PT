using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

using Random = UnityEngine.Random;

public class EnemyCenterManager : MonoBehaviour
{
    // 行動ステート
    public enum ActionState
    {
        Approach = 0,       // 接近モード
        Wait,               // 待機モード
        Attack,             // 攻撃モード
        Exit                // 退場モード
    }

    // 敵の種類
    public enum GroupType
    {
        Common,             // 一般エネミー
        Leader,             // 中ボス
        Boss,               // ボス
    }

    // 敵の移動量などを計算するクラス
    [SerializeField] AI_ActManager actManager;
    // プレイヤー間の距離
    [SerializeField] FloatReactiveProperty distance = new FloatReactiveProperty(0.0f);
    // 攻撃モードまでの待機時間
    [SerializeField] FloatReactiveProperty waitCount = new FloatReactiveProperty(0.0f);
    // 攻撃可能かどうかを管理するBool型プロパティ
    public BoolReactiveProperty attackFlg = new BoolReactiveProperty(false);
    // 行動ステートのプロパティ
    public ActStateReactiveProperty actProp = new ActStateReactiveProperty();

    [SerializeField] private Transform playerTrans;         // プレイヤーの座標
    [SerializeField] private Vector3 movePos;               // 移動ベクトル
    [SerializeField] private float maxDistance;             // プレイヤーとの最大接近距離
    [SerializeField] private float waitTimeLimit = 1.0f;    // 待機モードの最大遅延時間
    [SerializeField] private float atkTimeLimit = 1.0f;     // 攻撃モード時の攻撃する最大時間
    [SerializeField] private float velocityMag = 0.99f;     // 減速倍率
    [SerializeField] private float moveSpeed;               // 移動速度
    [SerializeField] private Rigidbody centerRigid;
    public ActionState actState;
    public GroupType groupType;

    Subject<int> apprSubject = new Subject<int>();
    Subject<int> waitSubject = new Subject<int>();
    Subject<int> atkSubject = new Subject<int>();
    Subject<int> escSubject = new Subject<int>();

    void Awake()
    {
        // プレイヤーの座標を取得
        playerTrans = GameManagement.Instance.playerTrans;
        // AI処理クラスのコンポーネント取得
        actManager = GameManagement.Instance.actManager;
    }
    // Start is called before the first frame update
    void Start()
    {
        apprSubject.Subscribe(val =>
        {
            movePos = actManager.CalcApprMove(this.transform.position,moveSpeed);
        }).AddTo(this.gameObject);

        waitSubject.Subscribe(val =>
        {
            attackFlg.Value = true;
        }).AddTo(this.gameObject);

        atkSubject.Subscribe(val =>
        {
            attackFlg.Value = false;
        }).AddTo(this.gameObject);

        actProp.Where(_ => _ == ActionState.Approach)
             .Subscribe(_ =>
             {
                 apprSubject.OnNext(0);
             }).AddTo(this.gameObject);

        // 攻撃タイプの待機モード処理
        actProp.Where(_ => actProp.Value == ActionState.Wait)
             .Where(_ => attackFlg.Value == false)
             .Sample(TimeSpan.FromSeconds(0.1f))
             .Subscribe(_ =>
             {
                 waitSubject.OnNext(0);
             }).AddTo(this.gameObject);

        // 攻撃タイプの攻撃モード処理
        actProp.Where(_ => _ == ActionState.Attack)
            .Where(_ => attackFlg.Value == true)
            .Sample(TimeSpan.FromSeconds(3.0f))
            .Subscribe(_ =>
            {
                atkSubject.OnNext(0);
            }).AddTo(this.gameObject);

        switch (actState)
        {
            case ActionState.Approach:
                this.UpdateAsObservable()
                    .Where(_ => GameManagement.Instance.isPause.Value == false)
                    .Where(_ => actProp.Value == ActionState.Approach)
                    .Subscribe(_ => 
                    {
                        this.centerRigid.velocity *= velocityMag;
                    }).AddTo(this.gameObject);
                break;
            case ActionState.Wait:
                break;
            case ActionState.Attack:
                break;
            case ActionState.Exit:
                break;
            default:
                break;
        }

        this.UpdateAsObservable()
            .Where(p => GameManagement.Instance.isPause.Value == false)
            .Subscribe(p =>
            {
                /*
                  全敵キャラクター共通処理
               */

                distance.Value = actManager.CalcDistance(this.transform.position);
                // 各AIにより算出された移動量をもとに移動処理を行う
                this.transform.position += (movePos * velocityMag) * Time.deltaTime;
                // 攻撃フラグがONになったら攻撃モードへ移行
                attackFlg.Where(_ => _ = attackFlg.Value == true && actProp.Value == ActionState.Wait)
                .Subscribe(_ =>
                {
                    actProp.Value = ActionState.Attack;
                }).AddTo(this.gameObject);
                if (actProp.Value == ActionState.Attack || actProp.Value == ActionState.Wait)
                {
                    movePos = Vector3.zero;
                }
            }).AddTo(this.gameObject);
        // 【接近モード移行イベント】
        // 敵が最大接近距離よりも遠ければ接近モードへ移行する
        distance.Where(_ => _ >= Mathf.Pow(maxDistance, 2))
            .Subscribe(_ =>
            {
                 // 減速移動量の設定
                 velocityMag = 0.99f;
                 // AIモードを接近モードに
                 actProp.Value = ActionState.Approach;
            }).AddTo(this.gameObject);

        // 【待機モード移行イベント】
        // 敵が最大接近距離に到達したら減速し次の行動を待つ
        distance.Where(_ => _ <= Mathf.Pow(maxDistance, 2))
            .Where(_ => actProp.Value == ActionState.Approach)
            .Subscribe(_ =>
            {
                 // 減速移動量の設定
                 velocityMag = 0.66f;
                 // AIモードを待機モードに
                 actProp.Value = ActionState.Wait;
            }).AddTo(this.gameObject);
    }
}

// 敵AI専用のカスタムプロパティ
[System.Serializable]
public class ActStateReactiveProperty : ReactiveProperty<EnemyCenterManager.ActionState>
{
    public ActStateReactiveProperty() { }
    public ActStateReactiveProperty(EnemyCenterManager.ActionState initialValue) : base(initialValue) { }
}