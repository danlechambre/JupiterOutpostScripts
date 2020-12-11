using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMover : MonoBehaviour
{
    private bool stopped;
    private float stopTimeElapsed;

    [SerializeField] Vector3 movementVector = new Vector3(0f, 0f, 0f);
    [SerializeField] float period = 2f;
    [SerializeField] float stopTime = 3.0f;

    private float movementFactor;

    const float tau = Mathf.PI * 2;
    private Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if (!stopped)
        {
            Move();
        }
        else
        {
            stopTimeElapsed += Time.deltaTime;
            if (stopTimeElapsed > stopTime)
            {
                stopped = false;
                stopTimeElapsed = 0f;
            }
        }
    }

    private void Move()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period;

        float rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Door collision detected");
        stopped = true;
    }
}
