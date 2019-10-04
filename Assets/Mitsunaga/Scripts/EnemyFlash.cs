using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    [SerializeField]
    Renderer enemyRenderer;

    [SerializeField]
    bool isDamage;
    
    void Start()
    {
        
    }
    void Update()
    {
        if (isDamage)
        {
            enemyRenderer.material.SetInt("_IsDamage", 1);
        }
        else
        {
            enemyRenderer.material.SetInt("_IsDamage", 0);

        }
    }
}
