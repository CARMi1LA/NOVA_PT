using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WallManager : MonoBehaviour
{
    // 敵のシールドクラス
    // 挙動はプレイヤーが発射した弾が衝突したら消滅するのみ
    // Start is called before the first frame update
    void Start()
    {
        this.OnTriggerEnterAsObservable()
            .Where(c => c.gameObject.tag == "Bullet")
            .Subscribe(c => 
            {
                // 弾のコンポーネント取得
                BulletManager bullet;
                bullet = c.gameObject.GetComponent<BulletManager>();
                if (bullet.shootChara == BulletManager.ShootChara.Player)
                {
                    bullet.bulletState = BulletManager.BulletState.Destroy;
                }
            }).AddTo(this.gameObject);
    }
}
