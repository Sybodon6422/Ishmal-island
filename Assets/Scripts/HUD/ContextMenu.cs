using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject contextMenu;
    [SerializeField] private TextMeshProUGUI nameText, weightText;
    [SerializeField] private Transform craftingTransform, craftingHolder;
    [SerializeField] private GameObject CTXcrafting, CTXpickup, CTXcontainer, CTXequip, CTXtake, CTXdrop;

    private WorldItem worldItem;
    private Inventory contextInventory;
    private Transform contextTransform;
    private List<CraftingMenuItem> activeCraftingRecipes;

    private void Start()
    {
        contextMenu.SetActive(false);
    }

    public void OpenMenu(Furnace furnace, Vector3 position)
    {
        contextMenu.SetActive(true);
        contextMenu.transform.position = position;
        nameText.text = "Furnace";
        weightText.text = "";
        craftingTransform.gameObject.SetActive(false);
        CTXpickup.SetActive(false);
        CTXcontainer.SetActive(true);
        contextInventory = furnace.furnaceInventory;
        contextTransform = furnace.transform;
        AudioManager.I.PlaySound("UIClick");
    }

    public void OpenMenu(WorldItem groundItem, WorldItem heldItem, Vector3 position)
    {
        contextMenu.SetActive(true);
        contextMenu.transform.position = position;
        nameText.text = groundItem.linkedItem.GetItemName();
        float roundedWeight = Mathf.Round(groundItem.itemWeight * 100f) / 100f;
        weightText.text = roundedWeight.ToString();
        CTXpickup.SetActive(true);
        CTXcontainer.SetActive(false);
        worldItem = groundItem;
        PopulateCraftingMenu(groundItem, heldItem);
        if (craftingTransform.childCount <= 0)
        {
            craftingTransform.gameObject.SetActive(false);
        }
        AudioManager.I.PlaySound("UIClick");
    }

    public void CloseMenu()
    {
        contextMenu.SetActive(false);
        contextInventory = null;
        contextTransform = null;
    }

    public void OpenCraftingMenu()
    {
        // TODO: Implement the functionality to open the crafting menu
    }

    public void OpenContainerMenu()
    {
        GameHUD.I.OpenNewContainer(contextInventory, contextTransform);
    }

    public void OpenContainerMenu(Inventory inventory, Transform containerLocation)
    {
        GameHUD.I.OpenNewContainer(inventory, containerLocation);
    }

    void PopulateCraftingMenu(WorldItem groundItem, WorldItem heldItem)
    {
        activeCraftingRecipes = new List<CraftingMenuItem>();
        for (int i = 0; i < craftingHolder.childCount; i++)
        {
            Destroy(craftingHolder.GetChild(i).gameObject);
        }

        if (!heldItem)
        {
            craftingTransform.gameObject.SetActive(false);
        }
        else
        {
            craftingTransform.gameObject.SetActive(true);
            var recipes = GameManager.I.craftDatabase.GetMatchingRecipes(groundItem, heldItem);

            if (recipes.Count <= 0)
            {
                craftingTransform.gameObject.SetActive(false);
            }

            for (int i = 0; i < recipes.Count; i++)
            {
                int recipeCount = 0;
                for (int z = 0; z < recipes.Count; z++)
                {
                    if (recipes[i] == recipes[z])
                    {
                        recipeCount++;
                        if (recipeCount > 1)
                        {
                            recipes.RemoveAt(i);
                        }
                    }
                }
                var go = Instantiate(CTXcrafting, craftingHolder);
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                var button = go.GetComponentInChildren<Button>();
                CraftingMenuItem craftMenuItem = new CraftingMenuItem(groundItem, heldItem, recipes[i], text, button);
                activeCraftingRecipes.Add(craftMenuItem);
            }
        }
    }

    public void ExamineItem()
    {
        GameHUD.I.consoleBox.PrintToConsole("It's a " + worldItem.linkedItem.name + ". It weights " + worldItem.GetItemWeight());
    }

    public void Pickup()
    {
        PlayerLocomotion.I.inventory.AddToInventory(worldItem);
        contextMenu.SetActive(false);
        Destroy(worldItem.gameObject);
    }
}
