using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "ShopDataList",
  menuName = "ScriptableObject/ShopDataList",
  order = 1)
]
public class ShopDataList : ScriptableObject
{
    public List<ShopData> dataList_Shop = new List<ShopData>();
}
