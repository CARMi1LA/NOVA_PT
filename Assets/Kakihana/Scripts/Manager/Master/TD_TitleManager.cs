using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class TD_TitleManager : MonoBehaviour
{
    // 一定速度で回転

    [SerializeField]
    Vector3 rotSpeed;

    public BoolReactiveProperty isClick = new BoolReactiveProperty(false);
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable()
        .Where(_ => isClick.Value == false)
        .Subscribe(_ =>
        {
            //this.transform.localEulerAngles += rotSpeed * Time.deltaTime;
            if (Input.GetButton("Button_A") == true || Input.GetButtonDown("Fire1") == true)
            {
                isClick.Value = true;
            }
        }).AddTo(this.gameObject);

        this.UpdateAsObservable()
        .Where(_ => isClick.Value == true)
        .Subscribe(_ =>
        {
            SceneManager.LoadScene("05 StageTD Mitsunaga");
        }).AddTo(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
