using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TDBulletFormList", menuName = "TDScriptable/TDBulletFormList", order = 0)]
public class TDBulletFormList : ScriptableObject
{
    // 陣営によって弾のカラーを変更する
    [ColorUsage(false, true)] public Color playerColor;
    [ColorUsage(false, true)] public Color enemyColor;
    // BulletData.BulletParentListから陣営に合わせて弾のカラーを返す
    public Color FormParent(TDBulletData.BulletParentList parent)
    {

        if(parent == TDBulletData.BulletParentList.Player)
        {
            return playerColor;
        }
        else
        {
            return enemyColor;
        }
    }

    // タイプによって弾の形を変更する
    public List<TDBulletForm> bFormList = new List<TDBulletForm>();
    // BulletData.BulletTypeList からタイプに合わせて弾の形を返す
    public TDBulletForm FormType(TDBulletData.BulletTypeList type)
    {
        TDBulletForm bForm = new TDBulletForm();

        foreach(var form in bFormList)
        {
            if(form.bType == type)
            {
                bForm = form;
            }
        }

        return bForm;
    }
}

[System.Serializable]
public class TDBulletForm
{
    // 弾のタイプ
    public TDBulletData.BulletTypeList bType;
    public Vector3 size;  // 弾の大きさ
}

