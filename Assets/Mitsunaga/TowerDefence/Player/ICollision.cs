using UnityEngine;

public interface ICollision
{
    // 衝突した場合の反発処理のインターフェイス
    // 相手のポジションから反発する方向を計算する
    void HitCollision(Vector3 targetPos);
}
