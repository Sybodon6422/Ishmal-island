using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilize : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private float waitTime = 2;
    void FixedUpdate()
    {
        if(waitTime <= 0)
        {
            rb.isKinematic = true;
            Destroy(this);
        }

        if(rb.velocity.magnitude < 5)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            waitTime = 2;
        }
    }
}
