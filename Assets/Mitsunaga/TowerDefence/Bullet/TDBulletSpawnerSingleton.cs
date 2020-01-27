using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDBulletSpawnerSingleton<T> : MonoBehaviour where T : TDBulletSpawnerSingleton<T>
{
    // 通常弾生成クラス（TDBulletSpawner）参照用シングルトン

    // クラス名の指定
    protected static readonly string[] findTags =
    {
        "TDBulletSpawner",
    };
    // 呼び出し
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogWarning(typeof(T) + "is nothing");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        CheckInstance();
    }

    // クラスの重複がないかの確認
    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }
}
