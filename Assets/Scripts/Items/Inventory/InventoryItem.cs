using System;

[Serializable]
public class InventoryItem
{
    public ItemSO itemRef;
    public float itemWeight;
    public float itemQuality;

    public InventoryItem(ItemSO _itemRef, float quality,float weight)
    {
        itemRef = _itemRef;
        itemQuality = quality;
        itemWeight = weight;
    }
}