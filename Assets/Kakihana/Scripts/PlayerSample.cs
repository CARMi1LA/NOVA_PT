using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerSample : MonoBehaviour
{

    public Vector3 pos;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) == true)
        {
            pos.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            pos.x = 1;
        }
        else if (Input.GetKey(KeyCode.W) == true)
        {
            pos.z = 1;
        }
        else if (Input.GetKey(KeyCode.S) == true)
        {
            pos.z = -1;
        }
        else
        {
            pos = Vector3.zero;
        }

        this.transform.position += pos * speed * Time.deltaTime;
    }
}
