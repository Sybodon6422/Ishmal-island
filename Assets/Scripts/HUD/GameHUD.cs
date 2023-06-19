using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameHUD : MonoBehaviour
{
    #region singleton
    private static GameHUD _instance;
    public static GameHUD I { get { return _instance; } }
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
    #endregion

    public ContextMenu contextMenu;
    public ConsoleBox consoleBox;

    [SerializeField] TextMeshProUGUI craftTimeText;
    [SerializeField] Slider craftTimeSlider;

    PlayerLocomotion player;
    int UILayer;

    private List<ContainerMenu> openInventories = new List<ContainerMenu>();

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        craftTimeText.text = "";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLocomotion>();
    }

    public void SetCraftTimer(float craftTime)
    {
        currentTime = craftTime;

        craftTimeText.text = Mathf.Round(currentTime).ToString();
        finishedOkay = true;
        waiting = true;
    }
    float currentTime = 0;
    bool waiting = false;

    public void CancelCraft()
    {
        waiting = true;
        finishedOkay = false;
        craftTimeText.text = "";
        currentTime = 99;
    }

    bool finishedOkay = true;

    private void FixedUpdate()
    {
        if (waiting)
        {
            currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, 999);

            craftTimeText.text = Mathf.Round(currentTime).ToString();

            if (currentTime <= 0)
            {
                OnCraftFinished?.Invoke(finishedOkay);
                waiting = false;
            }
        }

        //count for one second then preform check
        foreach (var menu in openInventories)
        {
            if(Vector3.Distance(menu.GetContainerPosition(), player.transform.position) > 5)
            {
                menu.CloseMenu();
                openInventories.Remove(menu);
                return;
            }
        }
    }

    [SerializeField] GameObject inventoryFab;
    public void OpenNewContainer(Inventory linkedInventory, Transform containerPosition)
    {
        var go = Instantiate(inventoryFab, contextMenu.GetComponent<RectTransform>().position, Quaternion.identity,transform);
        go.GetComponent<ContainerMenu>().SetupContainerMenu(linkedInventory, containerPosition);

        openInventories.Add(go.GetComponent<ContainerMenu>());
        AudioManager.I.PlaySound("UIClick");
    }

    public void EnterBuildMode()
    {
        player.EnterBuildMode();
    }

    public delegate void CraftFinished(bool finishedOkay);
    public event CraftFinished OnCraftFinished;

    public void CheckUIForContext()
    {

    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public void CloseAllMenus()
    {
        contextMenu.CloseMenu();
        foreach(var menu in openInventories)
        {
            menu.CloseMenu();
        }
    }
}
