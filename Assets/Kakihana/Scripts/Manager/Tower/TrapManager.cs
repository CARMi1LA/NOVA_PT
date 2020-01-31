using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TrapManager : MonoBehaviour
{
    // スロートラップの奥行きリスト（トラップレベルアップでサイズ変更）
    public float[] trapSizeListZ;
    // スロートラップの横幅リスト（タワーレベルアップでサイズ変更）
    public float[] trapSizeListX;
    // トラップの奥行きサイズ
    public float trapSizeZ;
    // トラップの横幅サイズ
    public float trapSizeX;

    public Subject<Unit> lvUpSub = new Subject<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        lvUpSub.Subscribe(_ => 
        {
            this.transform.localScale = new Vector3(trapSizeX, 1, trapSizeZ);
        }).AddTo(this.gameObject);
    }
}
