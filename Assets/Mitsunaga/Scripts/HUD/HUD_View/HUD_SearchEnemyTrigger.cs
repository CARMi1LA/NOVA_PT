using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HUD_SearchEnemyTrigger : MonoBehaviour
{
    // 敵がコライダー内に入ったら、敵の位置を渡す

    [SerializeField,Header("HUDのモデル")]
    HUD_Model hModel;
    [SerializeField] PlayerManager pm;

    void Start()
    {
        this.OnTriggerEnterAsObservable()
            .Where(x => x.gameObject.tag == "Enemy")
            .Subscribe(value =>
            {
                hModel.SearchEnemyRP.Value = value.gameObject.transform.position;
            })
            .AddTo(this.gameObject);

        this.UpdateAsObservable()
            .Where(x => Input.GetKeyDown(KeyCode.E))
            .Subscribe(_ =>
            {
                hModel.SearchEnemyRP.Value = Random.insideUnitCircle;
                Debug.Log(hModel.SearchEnemyRP.Value);
            })
            .AddTo(this.gameObject);
    }
}
