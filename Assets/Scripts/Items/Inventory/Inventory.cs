using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    private List<InventoryItem> inventoryItems;

    public void InitializeInventory()
    {
        inventoryItems = new List<InventoryItem>();
    }

    public void OpenInventory()
    {
        InventoryManager.I.UpdateInventory(inventoryItems);
    }
    
    public List<InventoryItem> GetInventory(){return inventoryItems;}
    public void AddToInventory(ItemSO itemSO, float quality, float weight)
    {
        InventoryItem newItem = new InventoryItem(itemSO, quality, weight);
        inventoryItems.Add(newItem);
    }
    public void AddToInventory(WorldItem worldItem)
    {
        InventoryItem newItem = new InventoryItem(worldItem.linkedItem, worldItem.itemQuality, worldItem.itemWeight);
        inventoryItems.Add(newItem);
    }

    public void RemoveItem(InventoryItem itemToRemove)
    {
        inventoryItems.Remove(itemToRemove);
    }

    public InventoryItem ItemMatch(ItemSO itemSO)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemRef == itemSO)
            {
                return item;
            }
        }
        Debug.Log("Item was not found!");
        return null;
    }

    public bool HasItem(ItemSO itemSO)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemRef == itemSO)
            {
                return true;
            }
        }

        return false;
    }
}
