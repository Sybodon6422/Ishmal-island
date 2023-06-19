using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemSO linkedItem;

    public float itemTemperature;
    public float ambientTemperature;
    public float itemWeight;
    public float itemQuality;
    private Collider targetCollider;

    private void Start()
    {
        targetCollider = GetComponent<Collider>();
        if (targetCollider == null)
        {
            targetCollider = GetComponentInChildren<Collider>();
        }
    }

    private void FixedUpdate()
    {
        itemTemperature = Mathf.Lerp(itemTemperature, ambientTemperature, 0.01f);
    }

    public void Setup(ItemSO _linkItem,float _Itemweight)
    {
        linkedItem = _linkItem;
        itemWeight = _Itemweight;

        gameObject.SetActive(true);
        Instantiate(linkedItem.GetWorldBody(), transform);

        targetCollider = GetComponent<Collider>();
        if (targetCollider == null)
        {
            targetCollider = GetComponentInChildren<Collider>();
        }

        PositiontargetCollider();
    }

    public float GetItemWeight() { return itemWeight; }
    public void UpdateItemWeight(float changeBy)
    {
        Debug.Log("Updated Weight by: " + changeBy);
        itemWeight = itemWeight - changeBy;
    }

    public bool AttemptPickUp(PlayerLocomotion player)
    {
        if (player.heldItem == null)
        {
            player.heldItem = this;
            transform.parent = player.carrySpot;
            transform.position = player.carrySpot.position;
            transform.rotation = player.carrySpot.rotation;
            //Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseEntirely()
    {
        Destroy(gameObject);
    }

    public void DropItem()
    {
        transform.parent = GameManager.I.GetItemContainer();

        PositiontargetCollider();
    }

    void PositiontargetCollider()
    {
        Vector3 rayOrigin = targetCollider.bounds.center;
        Vector3 rayDirection = -Vector3.up;
        Ray ray = new Ray(rayOrigin, rayDirection);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Position the collider at the hit point
            transform.position = hit.point;

            // Rotate the collider to match the surface normal
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }
}
