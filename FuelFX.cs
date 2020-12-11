using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelFX : MonoBehaviour
{
    private ParticleSystem chargingFX;

    private void Awake()
    {
        chargingFX = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        chargingFX.Stop();
        //foreach (var p in chargingFX)
        //{
        //    p.Stop();
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!chargingFX.isPlaying)
        {
            chargingFX.Play();
        }

        //foreach (var p in chargingFX)
        //{
        //    p.Play();
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!chargingFX.isStopped)
        {
            chargingFX.Stop();
        }

        //foreach (var p in chargingFX)
        //{
        //    p.Stop();
        //}
    }
}
