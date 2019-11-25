using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GamepadInput;

public class GameInputManager : MonoBehaviour
{
    // キー入力量を取得するクラス

    // ゲームパッドの番号
    [SerializeField] GamePad.Index[] inputPlayer;
    // どのプレイヤーでどのキーが押されたか
    [SerializeField] GamepadState[] inputState;

    public Subject<int> InitSubject = new Subject<int>();
    // Start is called before the first frame update
    void Start()
    {
        inputState[0] = GamePad.GetState(inputPlayer[0]);
        inputState[1] = GamePad.GetState(inputPlayer[1]);
        InitSubject.Subscribe(value => 
        {
            //switch (value)
            //{
            //    case 0:
            //        inputState[0] = GamePad.GetState(GamePad.Index.One);
            //        break;
            //    case 1:
            //        inputState[1] = GamePad.GetState(GamePad.Index.Two);
            //        break;
            //    default:
            //        break;
            //}
        }).AddTo(this.gameObject);

        // プレイヤー1のキー入力処理
        this.UpdateAsObservable()
            .Where(_ => inputState[0] == GamePad.GetState(GamePad.Index.One))
            .Subscribe(_ =>
            {
                // 左スティックの処理
                if (inputState[0].LeftStick)
                {
                    GameManagement.Instance.valueData1P.leftStickValue.x = inputState[0].LeftStickAxis.x;
                    GameManagement.Instance.valueData1P.leftStickValue.z = inputState[0].LeftStickAxis.y;
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
                if (inputState[0].RightStick)
                {
                    GameManagement.Instance.valueData1P.rightStickValue.x = inputState[0].rightStickAxis.x;
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

        // プレイヤー2のキー入力処理
        this.UpdateAsObservable()
            .Where(_ => inputState[1] != null)
            .Subscribe(_ =>
            {
                // 左スティックの処理
                if (inputState[1].LeftStick)
                {
                    GameManagement.Instance.valueData2P.leftStickValue.x = inputState[1].LeftStickAxis.x;
                    GameManagement.Instance.valueData2P.leftStickValue.z = inputState[1].LeftStickAxis.y;
                }

                // 左スティック（キーボード入力時）の処理
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    GameManagement.Instance.valueData2P.leftStickValue.x = Input.GetAxis("Horizontal");
                }
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    GameManagement.Instance.valueData2P.leftStickValue.z = Input.GetAxis("Vertical");
                }

                // 右スティックの処理
                if (inputState[1].RightStick)
                {
                    GameManagement.Instance.valueData2P.rightStickValue.x = inputState[1].rightStickAxis.x;
                }

                // 右スティック（キーボード入力時）の処理
                if (Input.GetAxis("Mouse X") != 0)
                {
                    GameManagement.Instance.valueData2P.rightStickValue.x = Input.GetAxis("Mouse X");
                }

                // Aボタンプッシュ時の処理
                if (inputState[1].A || Input.GetKey(KeyCode.E))
                {
                    GameManagement.Instance.valueData2P.pushBtnA.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData2P.pushBtnA.Value = false;
                }

                // Bボタンプッシュ時の処理
                if (inputState[1].B || Input.GetKey(KeyCode.Space))
                {
                    GameManagement.Instance.valueData2P.pushBtnB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData2P.pushBtnB.Value = false;
                }

                // Xボタンプッシュ時の処理
                if (inputState[1].X || Input.GetMouseButtonDown(1))
                {
                    GameManagement.Instance.valueData2P.pushBtnX.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData2P.pushBtnX.Value = false;
                }

                // Yボタンプッシュ時の処理
                if (inputState[1].Y || Input.GetKey(KeyCode.Q))
                {
                    GameManagement.Instance.valueData2P.pushBtnY.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData2P.pushBtnY.Value = false;
                }

                // RBボタンプッシュ時の処理
                if (inputState[1].RightShoulder || Input.GetMouseButtonDown(0))
                {
                    GameManagement.Instance.valueData2P.pushBtnRB.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData2P.pushBtnRB.Value = false;
                }

                // Startボタンプッシュ時の処理
                if (inputState[1].Start || Input.GetKey(KeyCode.Escape))
                {
                    GameManagement.Instance.valueData2P.pushBtnStart.Value = true;
                }
                else
                {
                    GameManagement.Instance.valueData2P.pushBtnStart.Value = false;
                }
            }).AddTo(this.gameObject);
    }
    public void Init(int index)
    {
        switch (index)
        {
            case 0:
                inputState[0] = GamePad.GetState(GamePad.Index.One);
                break;
            case 1:
                inputState[1] = GamePad.GetState(GamePad.Index.Two);
                break;
            default:
                break;
        }
    }
}
