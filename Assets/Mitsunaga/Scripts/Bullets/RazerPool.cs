using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;

public class RazerPool : ObjectPool<RazerManager>
{
    private readonly RazerManager _prefab;
    private readonly Transform _parentTransform;

    // コンストラクタ
    public RazerPool(Transform parent, RazerManager prefab)
    {
        _parentTransform = parent;
        _prefab = prefab;
    }

    // オブジェクトの追加生成時に実行
    protected override RazerManager CreateInstance()
    {
        // 生成
        RazerManager e = GameObject.Instantiate(_prefab);

        // 一箇所にまとめる
        e.transform.SetParent(_parentTransform);

        return e;
    }
}
