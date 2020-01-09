using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TrapManager : MonoBehaviour
{
    public float[] trapSpeedData;
    public float trapSpeed;

    // Start is called before the first frame update
    void Start()
    {
        trapSpeed = trapSpeedData[0];
    }
}
