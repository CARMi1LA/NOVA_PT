using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HitActManager : MonoBehaviour
{
    public float refrectPowor;      // 反発力

    // 当たり判定時の移動処理などを行う
    Subject<Vector3[]> HitRefrect = new Subject<Vector3[]>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


}
