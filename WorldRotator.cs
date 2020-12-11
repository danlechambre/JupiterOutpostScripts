using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotator : MonoBehaviour
{
    [SerializeField] private float minRotSpeed = 5f;
    [SerializeField] private float maxRotSpeed = 15f;
    private float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(minRotSpeed, maxRotSpeed);
    }

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
