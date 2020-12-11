using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBall : MonoBehaviour
{
    private Rigidbody rb;

    private bool dormant = false;
    private float timeToFire;
    private int fireTimersIndex = 0;
    private float delayedStartTimeElapsed = 0.0f;

    [SerializeField] private float[] fireTimers;
    [SerializeField] private float fakeGravity = -9.0f;
    [SerializeField] private float fireSpeed = 5.0f;
    [SerializeField] private float delayedStartTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetTimeToFire();
    }

    private void FixedUpdate()
    {
        if (!dormant)
        {
            ApplyGravity();
        }

        if (timeToFire <= Mathf.Epsilon)
        {
            Fire();
            SetTimeToFire();
        }
    }

    private void SetTimeToFire()
    {
        timeToFire = fireTimers[fireTimersIndex];
        if (fireTimersIndex < fireTimers.Length - 1)
        {
            fireTimersIndex++;
        }
        else
        {
            fireTimersIndex = 0;
        }
    }

    private void Update()
    {
        if (delayedStartTimeElapsed < delayedStartTimer)
        {
            delayedStartTimeElapsed += Time.deltaTime;
            return;
        }

        if (dormant)
        {
            timeToFire -= Time.deltaTime;
        }
    }

    private void Fire()
    {
        rb.AddForce(Vector3.up * fireSpeed, ForceMode.Impulse);
        dormant = false;
    }

    private void ApplyGravity()
    {
        Vector3 gravity = fakeGravity * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            rb.velocity = Vector3.zero;
            dormant = true;
        }

    }
}
