using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftingMenuItem
{
    TextMeshProUGUI nameText;
    ItemSO endItem;
    CraftRecipe craftingRecipe;
    WorldItem worldGroundItem, heldItem;
    Button linkedButton;

    public CraftingMenuItem(WorldItem _WorldGroundItem, WorldItem _HeldItem, CraftRecipe _CraftRecipe, TextMeshProUGUI _text, Button _button)
    {
        craftingRecipe = _CraftRecipe;
        nameText = _text;
        worldGroundItem = _WorldGroundItem;
        heldItem = _HeldItem;
        linkedButton = _button;
        nameText.text = _CraftRecipe.endProduct.name;
        endItem = _CraftRecipe.endProduct;

        linkedButton.onClick.AddListener(Craft);
    }

    public void Craft()
    {
        GameHUD.I.CancelCraft();
        if (!CanCraft()) { return; }
        GameHUD.I.OnCraftFinished += FinishedCraft;
        GameHUD.I.SetCraftTimer(GetCraftTime());
        GameHUD.I.contextMenu.CloseMenu();
    }

    private bool CanCraft()
    {
        if (worldGroundItem.linkedItem != craftingRecipe.item1)
        {
            if (!ValidateOneB()) { return false; }
            if (!ValidateTwoB()) { return false; }
        }
        else
        {
            if (!ValidateOne()) { return false; }
            if (!ValidateTwo()) { return false; }
        }

        return true;
    }

    float GetCraftTime()
    {
        if (craftingRecipe.requiredToolType == ItemSpecialProperty.NoType)
        {
            var newTime = craftingRecipe.crafTime - (
                (heldItem.linkedItem.GetToolTier() - craftingRecipe.requiredTier) * 2
                );
            if (newTime == 0) { return craftingRecipe.crafTime; }
            return newTime;
        }
        else
        {
            return craftingRecipe.crafTime;
        }
    }

    void FinishedCraft(bool finishedOkay)
    {
        GameHUD.I.OnCraftFinished -= FinishedCraft;
        if (!finishedOkay) { Debug.Log("Did not finish okay"); return; }
        if (worldGroundItem.linkedItem != craftingRecipe.item1)
        { //need switch
            if (EvaluateOneB() && EvaluateTwoB())
            {
                GameManager.I.CreateWorldItem(endItem, worldGroundItem.transform.position, worldGroundItem.transform.rotation);
            }
            else { Debug.Log("could not craft"); return; }
        }
        else
        {
            if (EvaluateOne() && EvaluateTwo())
            {
                GameManager.I.CreateWorldItem(endItem, worldGroundItem.transform.position, worldGroundItem.transform.rotation);
            }
            else { Debug.Log("could not craft"); return; }
        }
        if (craftingRecipe.item2)
        {
            GameHUD.I.consoleBox.PrintToConsole("Crafted: " + endItem.name + "   Used: " + craftingRecipe.useItem1 + " " + craftingRecipe.item1.name + " and: " + craftingRecipe.useItem2 + " " + craftingRecipe.item2.name);
        }
        else
        {
            GameHUD.I.consoleBox.PrintToConsole("Crafted: " + endItem.name + "   Used: " + craftingRecipe.useItem1 + " " + craftingRecipe.item1.name);
        }
    }

    bool EvaluateOne()
    {
        if (craftingRecipe.useItem1 == 0) { return true; }
        else if (craftingRecipe.useItem1 == worldGroundItem.GetItemWeight()) { worldGroundItem.UseEntirely(); return true; }
        else if (craftingRecipe.useItem1 > worldGroundItem.GetItemWeight()) { GameHUD.I.consoleBox.PrintToConsole("Not Enough material to craft"); return false; }
        else if (craftingRecipe.useItem1 < worldGroundItem.GetItemWeight()) { worldGroundItem.itemWeight -= craftingRecipe.useItem1; return true; }
        Debug.Log("Passed through one and failed"); return false;
    }

    bool EvaluateTwo()
    {
        if (craftingRecipe.useItem2 == 0) { return true; }
        else if (craftingRecipe.useItem2 == heldItem.GetItemWeight()) { PlayerLocomotion.I.RemoveCarriedObject(); return true; }
        else if (craftingRecipe.useItem2 > heldItem.GetItemWeight()) { return false; }
        else if (craftingRecipe.useItem2 < heldItem.GetItemWeight()) { heldItem.itemWeight -= craftingRecipe.useItem2; return true; }
        Debug.Log("Passed through two and failed"); return false;
    }

    bool EvaluateOneB()
    {
        if (craftingRecipe.useItem2 == 0) { return true; }
        else if (craftingRecipe.useItem2 == worldGroundItem.GetItemWeight()) { worldGroundItem.UseEntirely(); return true; }
        else if (craftingRecipe.useItem2 > worldGroundItem.GetItemWeight()) { GameHUD.I.consoleBox.PrintToConsole("Not Enough material to craft"); return false; }
        else if (craftingRecipe.useItem2 < worldGroundItem.GetItemWeight()) { worldGroundItem.itemWeight -= craftingRecipe.useItem2; return true; }
        Debug.Log("Passed through one secondary evaluation and failed"); return false;
    }

    bool EvaluateTwoB()
    {
        if (craftingRecipe.useItem1 == 0) { return true; }
        else if (craftingRecipe.useItem1 == heldItem.GetItemWeight()) { PlayerLocomotion.I.RemoveCarriedObject(); return true; }
        else if (craftingRecipe.useItem1 > heldItem.GetItemWeight()) { return false; }
        else if (craftingRecipe.useItem1 < heldItem.GetItemWeight()) { heldItem.itemWeight -= craftingRecipe.useItem2; return true; }
        Debug.Log("Passed through two secondary evaluation and failed"); return false;
    }

    bool ValidateOne()
    {
        if (craftingRecipe.useItem1 == 0) { return true; }
        else if (craftingRecipe.useItem1 == worldGroundItem.GetItemWeight()) { return true; }
        else if (craftingRecipe.useItem1 > worldGroundItem.GetItemWeight()) { GameHUD.I.consoleBox.PrintToConsole("Not Enough material to craft"); return false; }
        else if (craftingRecipe.useItem1 < worldGroundItem.GetItemWeight()) { return true; }

        return false;
    }
    bool ValidateTwo()
    {
        if (craftingRecipe.useItem2 == 0) { return true; }
        else if (craftingRecipe.useItem2 == heldItem.GetItemWeight()) { PlayerLocomotion.I.RemoveCarriedObject(); return true; }
        else if (craftingRecipe.useItem2 > heldItem.GetItemWeight()) { return false; }
        else if (craftingRecipe.useItem2 < heldItem.GetItemWeight()) { return true; }

        return false;
    }
    bool ValidateOneB() 
    {
        if (craftingRecipe.useItem2 == 0) { return true; }
        else if (craftingRecipe.useItem2 == worldGroundItem.GetItemWeight()) { return true; }
        else if (craftingRecipe.useItem2 > worldGroundItem.GetItemWeight()) { return false; }
        else if (craftingRecipe.useItem2 < worldGroundItem.GetItemWeight()) { return true; }

        return false;
    }
    bool ValidateTwoB()
    {
        if (craftingRecipe.useItem1 == 0) { return true; }
        else if (craftingRecipe.useItem1 == heldItem.GetItemWeight()) { return true; }
        else if (craftingRecipe.useItem1 > heldItem.GetItemWeight()) { return false; }
        else if (craftingRecipe.useItem1 < heldItem.GetItemWeight()) { return true; }

        return false;
    }
}
