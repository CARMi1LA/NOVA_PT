using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputValueData2P : MonoBehaviour
{
    // 入力された情報を保存するクラス
    // プレイヤーはこのクラスに入っているデータから各種処理を行う

    // プレイヤー2のデータ //

    // 左スティックの入力値（処理：プレイヤー移動）
    public Vector3 leftStickValue;
    // 右スティックの入力値（処理：視点移動）
    public Vector3 rightStickValue;
    // Aボタンの入力検知（処理：アクションキー）
    public BoolReactiveProperty pushBtnA;
    // Bボタンの入力検知（処理：回避）
    public BoolReactiveProperty pushBtnB;
    // Xボタンの入力検知（処理：スキル使用）
    public BoolReactiveProperty pushBtnX;
    // Yボタンの入力検知（処理：アルティメット）
    public BoolReactiveProperty pushBtnY;
    // R1ボタンの入力検知（処理：通常攻撃）
    public BoolReactiveProperty pushBtnRB;
    // L1ボタンの入力検知（未割り当て）
    public BoolReactiveProperty pushBtnLB;
    // Startボタンの入力検知（処理：ポーズ画面移行など）
    public BoolReactiveProperty pushBtnStart;
}
