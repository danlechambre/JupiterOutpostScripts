using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSFXController : MonoBehaviour
{
    private ShipController shipController;

    [SerializeField] private ParticleSystem mainThrustParticles;
    [SerializeField] private ParticleSystem leftThrustParticles;
    [SerializeField] private ParticleSystem rightThrustParticles;

    private void Awake()
    {
        shipController = FindObjectOfType<ShipController>();
    }

    private void Start()
    {
        if (mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Stop();
        }

        if (leftThrustParticles.isPlaying)
        {
            leftThrustParticles.Stop();
        }

        if (rightThrustParticles.isPlaying)
        {
            rightThrustParticles.Stop();
        }
    }

    public void StartMainThrustFX()
    {
        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }

    public void StopMainThrustFX()
    {
        if (!mainThrustParticles.isStopped)
        {
            mainThrustParticles.Stop();
        }
    }

    public void StartSideThrustFX(float torque)
    {
        if (torque < 0 && !leftThrustParticles.isPlaying)
        {
            leftThrustParticles.Play();
        }

        if (torque > 0 && !rightThrustParticles.isPlaying)
        {
            rightThrustParticles.Play();
        }
    }

    public void StopSideThrust()
    {
        if (!leftThrustParticles.isStopped)
        {
            leftThrustParticles.Stop();
        }

        if (!rightThrustParticles.isStopped)
        {
            rightThrustParticles.Stop();
        }
    }
}
