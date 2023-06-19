using System;
using UnityEngine;

[Serializable]
public class CraftRecipe
{
    public ItemSpecialProperty requiredToolType;

    public ItemSO item1;
    public float useItem1;
    public ItemSO item2;
    public float useItem2;

    public ItemSO endProduct;

    public float crafTime;
    public int requiredTier;
}
