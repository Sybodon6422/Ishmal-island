using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPopulate : MonoBehaviour
{
    public ItemSO item1, item2;

    public Vector2 distanceMinMax;
    public Vector3 worldPosition;
    public int ammount;
    public LayerMask layerMask;
    void Start()
    {
        for (int i = 0; i < ammount; i++)
        {
            RaycastHit hit;

            var randomPos = new Vector3(Random.Range(distanceMinMax.x, distanceMinMax.y), 20, Random.Range(distanceMinMax.x, distanceMinMax.y)) + worldPosition;

            if (Physics.Raycast(randomPos, -transform.up, out hit, 40, layerMask))
            {
                Vector3 spawnPos = hit.point;
                spawnPos.y += 1;
                if(Random.Range(0,100) <= 85)
                {
                    GameManager.I.CreateWorldItem(item1, hit.point, Quaternion.identity);
                }
                else
                {
                    GameManager.I.CreateWorldItem(item2, hit.point, Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
