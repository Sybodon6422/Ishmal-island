using UnityEngine;
using TMPro;
public class HUDInventoryPiece : MonoBehaviour
{
    private InventoryItem item;

    public TextMeshProUGUI nameText, weightText,qualityText;

    public void Setup(InventoryItem _item)
    {
        item = _item;


        nameText.text = item.itemRef.GetItemName();
        weightText.text = item.itemWeight.ToString();
        qualityText.text = item.itemQuality.ToString();
    }
    public void Clicked()
    {

    }
}