using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMachine : MonoBehaviour
{
    [SerializeField] float windStrength = 5.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent.gameObject.layer == 8)
        {
            Rigidbody rb = other.transform.parent.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * windStrength, ForceMode.Acceleration);
        }
    }
}