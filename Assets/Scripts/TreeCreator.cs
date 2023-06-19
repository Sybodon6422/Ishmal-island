using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCreator : MonoBehaviour
{
    [SerializeField] GameObject treeOBJ;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector2 heightRandomness, widthRandomnes;

    public int ammountOfTrees;
    public float forestSize;
    public void CreateTrees()
    {
        for (int i = 0; i < ammountOfTrees; i++)
        {
            RaycastHit hit;

            Vector3 randomPos = RandomCircle(transform.position, forestSize);

            if (Physics.Raycast(randomPos, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                var placePosition = hit.point;
                placePosition.y -= .1f;
                var go = Instantiate(treeOBJ, placePosition, Quaternion.FromToRotation(Vector3.up, hit.normal),transform);
                Tree treeComp = go.GetComponentInChildren<Tree>();

                treeComp.Create(Random.Range(heightRandomness.x, heightRandomness.y), Random.Range(widthRandomnes.x, widthRandomnes.y));

            }
        }
    }

    Vector3 RandomCircle(Vector3 center, float _radius)
    {
        var radius = Random.Range(0, _radius);
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = transform.position.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}