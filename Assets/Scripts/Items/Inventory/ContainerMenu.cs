using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerMenu : MonoBehaviour
{
    private Transform containerPosition;
    public Vector3 GetContainerPosition() { return containerPosition.position; }
    [SerializeField] private Transform inventoryListHolder;

    public void SetupContainerMenu(Inventory linkedInventory, Transform _containerPosition)
    {
        containerPosition = _containerPosition;
        UpdateInventory(linkedInventory.GetInventory());
    }   

    public void UpdateInventory(List<InventoryItem> inventoryItems)
    {
        for (int i = 0; i < inventoryListHolder.childCount; i++)
        {
            Destroy(inventoryListHolder.GetChild(i).gameObject);
        }
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            var go = Instantiate(InventoryManager.I.GetInventoryFab(),inventoryListHolder);
            go.GetComponent<HUDInventoryPiece>().Setup(inventoryItems[i]);
        }
    }

    public void CloseMenu()
    {
        Destroy(gameObject);
    }

}
