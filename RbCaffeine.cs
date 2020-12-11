using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbCaffeine : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void FixedUpdate()
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }
    }
}
