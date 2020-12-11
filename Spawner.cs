using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    CheckpointController cpController;
    CameraController camController;

    Rigidbody rb;

    [SerializeField] private float spawnPaddingY;

    private void Awake()
    {
        cpController = FindObjectOfType<CheckpointController>();
        camController = FindObjectOfType<CameraController>();
        rb = GetComponent<Rigidbody>();
    }

    public void SpawnShip()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        Vector3 spawnPos = cpController.CurrentLandingPad.transform.position;
        spawnPos.y += spawnPaddingY;

        transform.SetPositionAndRotation(spawnPos, Quaternion.Euler(Vector3.zero));
        GetComponent<Fuel>().ResetFuelUsedSinceLastCheckPoint();

        if (!camController.IsRefreshing)
        {
            camController.RefreshCamera();
        }
    }
}
