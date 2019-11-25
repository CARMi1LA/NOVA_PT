using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameInputManager : MonoBehaviour
{
    // キー入力量を取得するクラス

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
                // 左スティックの処理
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    GameManagement.Instance.valueData1P.leftStickValue.x = Input.GetAxis("Horizontal");
                }
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    GameManagement.Instance.valueData1P.leftStickValue.x = Input.GetAxis("Horizontal");
                }

                // 左スティック（キーボード入力時）の処理
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    GameManagement.Instance.valueData1P.leftStickValue.x = Input.GetAxis("Horizontal");
                }
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    GameManagement.Instance.valueData1P.leftStickValue.z = Input.GetAxis("Vertical");
                }

                // 右スティックの処理
                if (Input.GetAxisRaw("RightStickHorizontal") != 0)
                {
                    GameManagement.Instance.valueData1P.rightStickValue.x = Input.GetAxis("RightStickHorizontal");
                }

                // 右スティック（キーボード入力時）の処理
                if (Input.GetAxis("Mouse X") != 0)
                {
                    GameManagement.Instance.valueData1P.rightStickValue.x = Input.GetAxis("Mouse X");
                }

                // Aボタンプッシュ時の処理
                if (inputState[0].A || Input.GetKey(KeyCode.E))
                {
                    GameManagement.Instance.valueData1P.pushBtnA.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnA.Value = false;
                }

                // Bボタンプッシュ時の処理
                if (inputState[0].B || Input.GetKey(KeyCode.Space))
                {
                    GameManagement.Instance.valueData1P.pushBtnB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnB.Value = false;
                }

                // Xボタンプッシュ時の処理
                if (inputState[0].X || Input.GetMouseButtonDown(1))
                {
                    GameManagement.Instance.valueData1P.pushBtnX.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnX.Value = false;
                }

                // Yボタンプッシュ時の処理
                if (inputState[0].Y || Input.GetKey(KeyCode.Q))
                {
                    GameManagement.Instance.valueData1P.pushBtnY.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnY.Value = false;
                }

                // RBボタンプッシュ時の処理
                if (inputState[0].RightShoulder || Input.GetMouseButtonDown(0))
                {
                    GameManagement.Instance.valueData1P.pushBtnRB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnRB.Value = false;
                }

                // Startボタンプッシュ時の処理
                if (inputState[0].Start || Input.GetKey(KeyCode.Escape))
                {
                    GameManagement.Instance.valueData1P.pushBtnStart.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData1P.pushBtnStart.Value = false;
                }
            }).AddTo(this.gameObject);
}
