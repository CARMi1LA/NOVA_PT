using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameInputManager : MonoBehaviour
{
    // キー入力量を取得するクラス

    public Vector2 leftAxis;
    public Vector2 rightAxis;

    public Subject<int> InitSubject = new Subject<int>();
    // Start is called before the first frame update
    void Start()
    {
        InitSubject.Subscribe(value =>
        {
        }).AddTo(this.gameObject);

        // プレイヤー1のキー入力処理
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                leftAxis.x = Input.GetAxis("Horizontal");
                leftAxis.y = Input.GetAxis("Vertical");
                rightAxis.x = Input.GetAxis("RightStickHorizontal");

                GameManagement.Instance.valueData1P.leftStickValue.x = leftAxis.x;
                GameManagement.Instance.valueData1P.leftStickValue.z = leftAxis.y;
                GameManagement.Instance.valueData1P.rightStickValue.x = rightAxis.x;

                // 右スティック（キーボード入力時）の処理
                if (Input.GetAxis("Mouse X") != 0)
                {
                    GameManagement.Instance.valueData1P.rightStickValue.x = Input.GetAxis("Mouse X");
                }

                // Aボタンプッシュ時の処理
                if (Input.GetButton("Button_A") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnA.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnA.Value = false;
                }

                // Bボタンプッシュ時の処理
                if (Input.GetButton("Button_B") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnB.Value = false;
                }

                // Xボタンプッシュ時の処理
                if (Input.GetButton("Button_X") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnX.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnX.Value = false;
                }

                // Yボタンプッシュ時の処理
                if (Input.GetButton("Button_Y") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnY.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnY.Value = false;
                }

                // RBボタンプッシュ時の処理
                if (Input.GetButton("Button_RB") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnRB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnRB.Value = false;
                }

                // LBボタンプッシュ時の処理
                if (Input.GetButton("Button_LB") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnRB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnRB.Value = false;
                }

                // Startボタンプッシュ時の処理
                if (Input.GetButton("Button_Start") == true)
                {
                    GameManagement.Instance.valueData1P.pushBtnStart.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnStart.Value = false;
                }
            }).AddTo(this.gameObject);
    }
}
