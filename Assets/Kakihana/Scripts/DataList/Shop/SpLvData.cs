using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class SpLvData : MonoBehaviour
{
    public LevelData_Player playerLv;
    public LevelData_Tower[] towerLv;
    public LevelData_Skill skillLv;
    public LevelData_Ult ultLv;

    public Subject<Unit> SpLvInit = new Subject<Unit>();
    // Start is called before the first frame update
    void Awake()
    {
        SpLvInit.Subscribe(_ =>
        {
            playerLv = new LevelData_Player();
            for (int i = 0; i > 4; i++)
            {
                towerLv[i] = new LevelData_Tower();
            }
            skillLv = new LevelData_Skill();
            ultLv = new LevelData_Ult();
            Debug.Log("LvInit");
        }).AddTo(this.gameObject);
    }
}
