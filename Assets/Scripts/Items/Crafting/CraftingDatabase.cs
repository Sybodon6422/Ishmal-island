using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Database", menuName = "Ismhal/Databases/New Crafting Database", order = 1)]
public class CraftingDatabase : ScriptableObject
{
    public List<CraftRecipe> recipes;

    public List<CraftRecipe> GetMatchingRecipes(WorldItem worldItem, WorldItem HeldItem)
    {
        List<CraftRecipe> matchingRecipes = new List<CraftRecipe>();
        for (int i = 0; i < recipes.Count; i++)
        {
            if (CheckRecipe(recipes[i],worldItem,HeldItem))
            {
                matchingRecipes.Add(recipes[i]); 
            }
        }

        return matchingRecipes;
    }

    bool CheckRecipe(CraftRecipe _recipe, WorldItem i1, WorldItem i2)
    {
        if (CheckWorldItem(_recipe, i1, i2))
        {
            if (!_recipe.item2)
            {
                if (CheckRecipeToolandLevel(_recipe, i1, i2))
                {
                    return true;
                }
            }
            else
            {
                if (CheckHeldItem(_recipe, i1, i2))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool CheckRecipeToolandLevel(CraftRecipe _recipe, WorldItem i1, WorldItem i2)
    {
        if(i2.linkedItem.HasSpecialProperty(_recipe.requiredToolType) && i2.linkedItem.GetToolTier() >= _recipe.requiredTier)
        {
            return true;
        }
        return false;
    }

    bool CheckWorldItem(CraftRecipe _recipe, WorldItem i1, WorldItem i2)
    {
        if(_recipe.item1 == i1.linkedItem)
        {
            return true;
        }
        return false;
    }
    bool CheckHeldItem(CraftRecipe _recipe, WorldItem i1, WorldItem i2)
    {
        if (_recipe.item2 == i2.linkedItem)
        {
            return true;
        }
        return false;
    }
}
