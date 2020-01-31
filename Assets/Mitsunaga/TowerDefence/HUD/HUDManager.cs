using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HUDManager : MonoBehaviour
{
    /*
    プレイヤーのHUDの管理
     * Health
     * Energy
     * Ultimate
     * Material
     * タワーのHealth
     * Wave数
     * ミニマップ
     * ボスのHP
    それぞれの処理をマスターデータとつなぐ
     */

    // プレイヤーのデータ
    [SerializeField] TDPlayerManager pManager;
    public TDPlayerData pData;
    // HUDのView
    // プレイヤーのキャンバス
    [SerializeField] HUDHealth      hHealth;        // ヘルスゲージ
    [SerializeField] HUDEnergy      hEnergy;        // エネルギーゲージ
    [SerializeField] HUDUltimate    hUltimate;      // アルティメットゲージ
    // カメラ左キャンバス
    [SerializeField] HUDMater       hMater;         // 所持マテリアル
    [SerializeField] HUDTowerHealth hTowerHealth;   // タワーのヘルスゲージ
    [SerializeField] HUDPlayerLevel hPlayerLevel;   // プレイヤーのパラメータ
    // カメラ右キャンバス
    [SerializeField] HUDWaveCount   hWaveCount;     // ウェーブ数
    [SerializeField] HUDWaveTime    hWaveTime;      // ウェーブの時間計測
    float startWaveTime = 0;
    // カメラ中央キャンバス
    [SerializeField] HUDBossBattle  hBossBattle;    // ボスのヘルスゲージ
    [SerializeField] HUDRespawnTime hRespawnTime;   // 死亡時のリスポーン表示

    // タワーのHP　マスターから取得する予定
    float[] towerHealth = new float[4];
    float towerMaxHealth = 100;

    int waveCount = 1;
    int maxCount = 6;

    void Awake()
    {
        // 初期設定 マスターからそれぞれ表示するデータを取得する
        //pData = GameManagement.Instance.playerData;

        // デバッグ用
        pData = pManager.pData;
    }
    void Start()
    {
        // ヘルス表示
        pData.pHealth
            .Subscribe(value =>
            {
                if(hHealth != null)
                {
                    hHealth.SetHealth(value, pData.pMaxHealth);
                }

            }).AddTo(this.gameObject);
        // エネルギー表示
        pData.pEnergy
            .Subscribe(value =>
            {
                if(hEnergy != null)
                {
                    hEnergy.SetEnergy(value, pData.pMaxEnergy);
                }

            }).AddTo(this.gameObject);
        // アルティメット表示
        pData.pUltimate
            .Subscribe(value =>
            {
                if(hUltimate != null)
                {
                    hUltimate.SetUltimate(value, pData.pMaxUltimate);
                }

            }).AddTo(this.gameObject);
        // 所持マテリアル表示
        GameManagement.Instance.mater
            .Subscribe(value =>
            {
                hMater.SetMater(value);

            }).AddTo(this.gameObject);
        // ウェーブの時間の表示
        GameManagement.Instance.gameState
            .Delay(System.TimeSpan.FromSeconds(0.01f))
            .Subscribe(_ =>
            {
                startWaveTime = GameManagement.Instance.masterTime;

            }).AddTo(this.gameObject);
        // 死亡時のリスポーン表示
        pManager.isDeath
            .Subscribe(value =>
            {
                hRespawnTime.SetRespawnTime(value);

            }).AddTo(this.gameObject);
        
        // 更新処理
        // タワー体力、ウェーブ数・時間の表示など
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                towerHealth[0] = GameManagement.Instance.blueTw.towerHp.Value;
                towerHealth[1] = GameManagement.Instance.redTw.towerHp.Value;
                towerHealth[2] = GameManagement.Instance.yellowTw.towerHp.Value;
                towerHealth[3] = GameManagement.Instance.greenTw.towerHp.Value;
                hTowerHealth.SetTowerHealth(100, towerHealth);
                hTowerHealth.SetTowerTarget(GameManagement.Instance.targetTw);
                hWaveCount.SetWaveCount(waveCount, maxCount);
                hWaveTime.SetWaveTime(GameManagement.Instance.masterTime, startWaveTime);
                hPlayerLevel.SetLevels(ShopManager.Instance.spLv);

            }).AddTo(this.gameObject);
    }
}
