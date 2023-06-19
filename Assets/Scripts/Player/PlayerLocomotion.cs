using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    GameControls gameControls;
    CharacterController cc;
    [SerializeField] float mouseSensitivity = .2f;
    [SerializeField] float speed = 3;

    PlayerSpecifications specs;
    Animator anim;

    public WorldItem heldItem;
    public Transform carrySpot;

    TerrainDeformation terraformer;

    private static PlayerLocomotion _instance;
    public static PlayerLocomotion I { get { return _instance; } }

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

        gameControls = new GameControls();
        cc = gameObject.GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    Vector2 mousePos;

    Vector2 movement;
    Vector2 mouseMovement;

    void Start()
    {
        gameControls.Default.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        gameControls.Default.Movement.canceled += ctx => movement = Vector2.zero;

        gameControls.Default.Click.performed += ctx => Attack();
        gameControls.Default.RightClick.performed += ctx => RightClick();
        gameControls.Default.Carry.performed += ctx => Enteract();
        gameControls.Default.SwitchMouse.performed += ctx => SwapMouseMode();

        gameControls.Default.Terraform.performed += ctx => Terraform();
        gameControls.Default.Flatten.performed += ctx => Flatten();
        gameControls.Default.SwapTerraform.performed += ctx => removing = !removing;

        gameControls.Default.DropItem.performed += ctx => DropItem();

        gameControls.Default.CloseAll.performed += ctx => DropItem();
        gameControls.Default.Inventory.performed += ctx => InventoryManager.I.ToggleInventory();

        gameControls.Default.MousePosition.performed += ctx => mousePos = ctx.ReadValue<Vector2>();

        gameControls.Default.MouseMovement.performed += ctx => mouseMovement = ctx.ReadValue<Vector2>();
        gameControls.Default.MouseMovement.canceled += ctx => mouseMovement = Vector2.zero;

        cam = Camera.main;
        specs = GetComponent<PlayerSpecifications>();
        inventory = new Inventory();
        inventory.InitializeInventory();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private Camera cam;
    public Inventory inventory;

    float verticalLookRotation;
    [SerializeField] LayerMask buildLayerMask;
    void FixedUpdate()
    {
        //player movement
        Vector3 fixedMovement = movement.y * transform.forward + movement.x * transform.right;
        fixedMovement *= speed;
        fixedMovement.y = -9.81f + (cc.velocity.y / 10);
        cc.Move(fixedMovement * Time.deltaTime);

        TerrainCheck();

        if (buildMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 16f, buildLayerMask))
            {
                if(Vector3.Distance(transform.position, hit.point) < 8)
                {
                    BuildingSystem.I.buildWorldPos = hit.point;
                }
            }
        }
    }

    private GameObject currentChunk;
    private void LateUpdate()
    {
            if (mouseModeUnlocked) { return; }
        transform.Rotate(Vector3.up * mouseMovement.x * mouseSensitivity);
        verticalLookRotation += mouseMovement.y * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cam.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void TerrainCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f, terralayerMask))
        {
            if (currentChunk)
            {
                if (hit.transform.gameObject == currentChunk)
                {
                    return;
                }
                else
                {
                    currentChunk = hit.transform.gameObject;
                    terraformer = currentChunk.GetComponent<TerrainDeformation>();
                    return;
                }
            }
            else
            {
                currentChunk = hit.transform.gameObject;
                terraformer = currentChunk.GetComponent<TerrainDeformation>();
                return;
            }
        }
    }

    public bool allowAttack = true;
    void Attack()
    {
        if (allowAttack)
        {
            allowAttack = false;
            anim.Play("Swing");
        }
    }

    public void MidSwing() {
        RaycastHit hit;
        if (!heldItem)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                hit.transform.GetComponent<IDamagable>()?.Damage(2, null, 0, specs);
            }
        }
        else
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, heldItem.linkedItem.GetToolReach()))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                hit.transform.GetComponent<IDamagable>()?.Damage(heldItem.linkedItem.GetToolDamage(), heldItem, heldItem.linkedItem.GetToolTier(), specs);
            }
        }
    }
    public void EndSwing()
    {
        allowAttack = true;
    }

    [SerializeField] Transform logCarryPosition;
    private List<Transform> logs = new List<Transform>();

    void Enteract()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.transform.GetComponent<WorldItem>())
            {
                heldItem = hit.transform.GetComponent<WorldItem>();
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.transform.parent = logCarryPosition;
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
                logs.Add(heldItem.transform);

                return;
            }
            else
            {
                hit.transform.GetComponent<IEnteractable>()?.Enteract(this);
                Debug.Log("Cast Enteract Attempt");
            }
        }
    }

    void TryItemPickUp()
    {

    }

    bool mouseModeUnlocked = false;
    void SwapMouseMode()
    {
        if (mouseModeUnlocked)
        {
            mouseModeUnlocked = false;
            Cursor.lockState = CursorLockMode.Locked;
            GameHUD.I.contextMenu.CloseMenu();
        }
        else
        {
            mouseModeUnlocked = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void RightClick()
    {
        //if(GameHUD.I.IsPointerOverUIElement())
        //{
        //    GameHUD.I.CheckUIForContext();
        //}
        //else
        //{
            CheckWorldForInteractions();
        //}
    }

    public void CheckWorldForInteractions()
    {
        RaycastHit hit;
            var castPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,1));
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray,out hit,4))
            {
                Debug.DrawRay(castPos, cam.transform.forward * 4, Color.yellow,1);
                if (hit.transform.GetComponent<WorldItem>())
                {
                    WorldItem hitItem = hit.transform.GetComponent<WorldItem>();
                    if (heldItem)
                    {
                        GameHUD.I.contextMenu.OpenMenu(hitItem, heldItem, mousePos);
                    }
                    else
                    {
                        GameHUD.I.contextMenu.OpenMenu(hitItem, null, mousePos);
                    }
                }
                else if(hit.transform.gameObject.GetComponent<ChunkRenderer>())
                {
                    var chunk = hit.transform.gameObject.GetComponent<ChunkRenderer>();
                }
                else if(hit.transform.gameObject.GetComponent<Furnace>() != null)
                {
                    GameHUD.I.contextMenu.OpenMenu(hit.transform.gameObject.GetComponent<Furnace>(),mousePos);
                }
            }
    }

    public void RemoveCarriedObject()
    {
        heldItem = null;
        Destroy(logCarryPosition.GetChild(0).gameObject);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!cc.isGrounded && hit.transform.tag == "Wall")
        {
            //walljump stuff
        }
        Rigidbody rb = hit.collider.attachedRigidbody;
        if(rb !=null && !rb.isKinematic)
        {
            rb.velocity += hit.moveDirection * .1f;
        }
    }
    public LayerMask terralayerMask;
    bool removing = false;
    void Terraform()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 8f, terralayerMask))
        {
            terraformer.ChangeHeight(hit.point,.1f,removing);
        }
    }
    void Flatten()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 8f, terralayerMask))
        {
            Vector3 flattenPos = transform.position + new Vector3(0,-1.08f,0);
            terraformer.MoveTowardsSetHeight(hit.point, .1f, flattenPos);
        }
    }

    void DropItem()
    {
        if (heldItem)
        {
        heldItem.DropItem();
        heldItem = null;
        }
    }

    public void EnterBuildMode()
    {
        buildMode = !buildMode;
    }
    bool buildMode = false;

    private void OnEnable()
    {
        gameControls.Enable();
    }

    private void OnDisable()
    {
        gameControls.Disable();
    }
}
