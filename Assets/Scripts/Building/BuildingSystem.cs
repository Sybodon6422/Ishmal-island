using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    private static BuildingSystem _instance;
    public static BuildingSystem I { get { return _instance; } }

    [SerializeField] Transform ghost;

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

    void Start()
    {
        
    }

    private bool isBuliding = false;


    public Vector3 buildWorldPos;

    void Update()
    {
        if (isBuliding)
        {
            ghost.position = buildWorldPos;
        }
        
    }

    void StartBuilding()
    {
        isBuliding = true;
    }

    void StopBuilding()
    {
        isBuliding = false;
    }
}
