using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TrapManager : MonoBehaviour
{
    public float[] trapSize;
    public float trapDefault;

    public Subject<Unit> lvUpSub = new Subject<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        lvUpSub.Subscribe(_ => 
        {
            this.transform.localScale = new Vector3(200, 1, trapDefault);
        }).AddTo(this.gameObject);
    }
}
