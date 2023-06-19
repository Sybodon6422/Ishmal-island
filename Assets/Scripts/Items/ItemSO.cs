using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Ismhal/New item", order = 1)]
public class ItemSO : ScriptableObject
{
    [SerializeField] List<ItemSpecialProperty> specialProperties;

    [SerializeField] string itemName;
    [SerializeField] float itemWeight;

    [SerializeField] GameObject worldBody;

    [Header("Tool Settings")]
    [SerializeField] float damage;

    [SerializeField] int toolTier;
    [SerializeField] float toolReach;

    public void GetContextOptions()
    {

    }

    public string GetItemName() { return itemName; }
    public float GetItemWeight() { return itemWeight; }
    public GameObject GetWorldBody() { return worldBody; }

    public float GetToolDamage() { return damage; }
    public int GetToolTier() { return toolTier; }
    public float GetToolReach() { return toolReach; }
    public List<ItemSpecialProperty> GetSpecialProperties() { return specialProperties; }
    public bool HasSpecialProperty(ItemSpecialProperty _property)
    {
        for (int i = 0; i < specialProperties.Count; i++)
        {
            if (specialProperties[i] == _property) { return true; }
        }
        return false;
    }
}