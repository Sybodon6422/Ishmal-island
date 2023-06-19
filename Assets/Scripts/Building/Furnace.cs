using UnityEngine;

public class Furnace : MonoBehaviour, IEnteractable
{
    public float fuel = 0f;
    public float furnaceTemp = 2500f;

    [SerializeField] private GameObject fireLight;
    private bool burning = false;
    [SerializeField] private float fuelBurnRate = 1;

    public Inventory furnaceInventory;

    private void Start() {
        furnaceInventory = new Inventory();
        furnaceInventory.InitializeInventory();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(burning)
        {
            fuel -= Mathf.Clamp(Time.deltaTime * (0.01f * fuelBurnRate),0, 99);
            if(fuel <= 0)
            {
                burning = false;
            }
            else
            {
                furnaceTemp = Mathf.Clamp(furnaceTemp + 3, 20, 2500);
            }
        }
        else
        {
            furnaceTemp = Mathf.Clamp(furnaceTemp - 1, 20, 2500);
        }
    }

    public void Enteract(PlayerLocomotion player)
    {
        if(player.heldItem)
        {
            if(player.heldItem.linkedItem.HasSpecialProperty(ItemSpecialProperty.Fuel))
            {
                Debug.Log("Adding fuel to furnace");
                fuel += player.heldItem.itemWeight;
                player.heldItem = null;
                return;
            }
        }
        else
        {
            burning = !burning;
            fireLight.SetActive(burning);
        }
    }
}
