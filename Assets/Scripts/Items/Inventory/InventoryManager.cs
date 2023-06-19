using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region singleton
    private static InventoryManager _instance;
    public static InventoryManager I { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    #endregion

    [SerializeField] GameObject inventoryFab;
    public GameObject GetInventoryFab(){return inventoryFab;}

    [SerializeField] Transform holder;
    [SerializeField] GameObject inventoryUI;
    void Start(){inventoryUI.SetActive(false);}

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!thisIsActive);
        thisIsActive = !thisIsActive;
        AudioManager.I.PlaySound("UIClick");
        UpdateInventory(PlayerLocomotion.I.inventory.GetInventory());
    }

    bool thisIsActive = false;
    public void UpdateInventory(List<InventoryItem> inventoryItems)
    {
        for (int i = 0; i < holder.childCount; i++)
        {
            Destroy(holder.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            var go = Instantiate(inventoryFab,holder);
            go.GetComponent<HUDInventoryPiece>().Setup(inventoryItems[i]);
        }
    }
}