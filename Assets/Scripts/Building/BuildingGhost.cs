using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    MeshRenderer ghostRenderer;
    MeshFilter filter;
    MeshCollider ghostCollider;
    private Material seeThroughMat;

    private GameObject ghostHolder;

    private void Start()
    {
        ghostRenderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        ghostCollider = GetComponent<MeshCollider>();

        seeThroughMat = ghostRenderer.material;
        ghostRenderer.material = seeThroughMat;
    }

    public void Setup(GameObject buliding)
    {
        ghostHolder = Instantiate(buliding, transform);

        ghostRenderer = ghostHolder.GetComponent<MeshRenderer>();
        filter = ghostHolder.GetComponent<MeshFilter>();
        ghostCollider = GetComponent<MeshCollider>();

        seeThroughMat = ghostRenderer.material;
        ghostRenderer.material = seeThroughMat;
    }
    private bool collisionTrigger = false;
    public bool CanBuild()
    {
        return !collisionTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionTrigger = true;
        seeThroughMat.color = Color.red;
    }
    private void OnTriggerExit(Collider other)
    {
        collisionTrigger = false;
        seeThroughMat.color = Color.green;
    }
}
