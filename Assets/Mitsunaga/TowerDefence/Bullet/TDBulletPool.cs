using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;

public class TDBulletPool : ObjectPool<TDBulletManager>
{
    // タワーディフェンスの通常弾のオブジェクトプール

    private readonly TDBulletManager _prefab;
    private readonly Transform _parentTransform;

    // コンストラクタ
    public TDBulletPool(Transform parent, TDBulletManager prefab)
    {
        _parentTransform = parent;
        _prefab = prefab;
    }

    // オブジェクトの追加生成時に実行
    protected override TDBulletManager CreateInstance()
    {
        // 生成
        TDBulletManager e = GameObject.Instantiate(_prefab);

        // 一箇所にまとめる
        e.transform.SetParent(_parentTransform);

        return e;
    }
}
