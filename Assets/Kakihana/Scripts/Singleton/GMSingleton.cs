using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMSingleton<T> : MonoBehaviour where T : GMSingleton<T>
{
    // GameManagement参照用シングルトン

    protected static readonly string[] findTags =
    {
        "GameManagement",
    };

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
