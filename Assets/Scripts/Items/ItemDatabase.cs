using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items Database", menuName = "Ismhal/Databases/New Item Database", order = 1)]
public class ItemDatabase : ScriptableObject
{
    public List<ItemSO> itemsDatabaseList;
}
