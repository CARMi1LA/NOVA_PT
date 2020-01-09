﻿using UnityEngine;

public interface ICollisionTD
{
    // 衝突した場合の反発処理のインターフェイス
    // 相手のポジションから反発する方向を計算する
    void HitCollision(TDList.ParentList parent, Vector3 targetPos);
}
