using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SoundManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] BGMs;

    // Start is called before the first frame update
    void Start()
    {
        GameManagement.Instance.starting.Subscribe(_ =>
        {
            audio.clip = BGMs[0];
            audio.Play();
        }).AddTo(this.gameObject);

        GameManagement.Instance.isClear.Where(_ => _ == true).Subscribe(_ => 
        {
            audio.clip = BGMs[2];
            audio.Play();
        }).AddTo(this.gameObject);
    }
}
