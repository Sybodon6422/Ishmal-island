using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager I { get { return _instance; } }

    public CraftingDatabase craftDatabase;
    public ItemDatabase itemDatabase;

    [SerializeField] GameObject worldItemFab;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    private Transform TreeContainer, ItemContainer;
    private void Start()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);

        TreeContainer = new GameObject("TreeContainer").transform;
        TreeContainer.parent = transform;

        ItemContainer = new GameObject("ItemContainer").transform;
        ItemContainer.parent = transform;
    }

    public void CreateWorldItem(ItemSO _Item, Vector3 location, Quaternion rotation)
    {
        var go = Instantiate(worldItemFab,location,rotation,ItemContainer);
        go.GetComponent<WorldItem>().Setup(_Item , _Item.GetItemWeight());
    }
    public void CreateWorldItem(ItemSO _Item, Vector3 location, Quaternion rotation, float itemWeight)
    {
        location.y = location.y + .1f;
        var go = Instantiate(worldItemFab, location, rotation);
        go.GetComponent<WorldItem>().Setup(_Item, itemWeight);
    }

    public Transform GetItemContainer()
    {
        return ItemContainer;
    }
    public Transform GetTreeContainer()
    {
        return TreeContainer;
    }
}
