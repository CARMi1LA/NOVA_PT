using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TDBulletFormChange : MonoBehaviour
{
    [SerializeField]
    TDBulletManager bManager;

    Vector3 originScale;

    void Awake()
    {
        originScale = this.transform.localScale;
        TDBulletFormList bFormList = Resources.Load<TDBulletFormList>("TDBulletFormList");

        bManager.initTrigger
            .Subscribe(value =>
            {
                // 陣営によってカラーを変更する
                this.GetComponent<Renderer>().material.SetColor("_MainColor", bFormList.FormParent(value.bParent));
                // タイプによって形を変更する
                SetFormType(bFormList.FormType(value.bType));

            }).AddTo(this.gameObject);
    }

    void SetFormType(TDBulletForm bForm)
    {
        // サイズ変更
        Vector3 scale = originScale;
        scale.x *= bForm.size.x;
        scale.y *= bForm.size.y;
        scale.z *= bForm.size.z;

        this.transform.localScale = scale;
    }
}
