using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrashHandler : MonoBehaviour
{
    DialogueUI dialogueUI;
    Spawner spawner;

    Rigidbody rb;

    private bool crashed;

    [SerializeField] float velocityThresholdLow;
    [SerializeField] float velocityThresholdHigh;
    [SerializeField] float maximumLandingAngle;

    private enum CrashSeverity
    {
        Light = 0,
        Heavy = 1,
        Major = 2,
        Lava = 3
    }

    public bool Crashed { get { return crashed; } }

    private void Awake()
    {
        dialogueUI = FindObjectOfType<DialogueUI>();

        spawner = FindObjectOfType<Spawner>();

        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (crashed) { return; }

        int cLayer = collision.gameObject.layer;
        if (cLayer == 9) // camera colliders
        {
            return;
        }

        float cVelocity = collision.relativeVelocity.sqrMagnitude;

        Quaternion shipRotation = gameObject.transform.rotation;
        float angle = Quaternion.Angle(shipRotation, Quaternion.identity);

        if (cVelocity > velocityThresholdHigh)
        {
            StartCoroutine(CrashCoroutine(CrashSeverity.Major));
        }
        else if (cVelocity > velocityThresholdLow)
        {
            StartCoroutine(CrashCoroutine(CrashSeverity.Heavy));
        }
        else if (cLayer == 13 || angle > maximumLandingAngle) // 13 is Environment
        {
            StartCoroutine(CrashCoroutine(CrashSeverity.Light));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (crashed) { return; }

        int cLayer = other.gameObject.layer;

        if (cLayer == 11) // 11 is Lava
        {
            StartCoroutine(CrashCoroutine(CrashSeverity.Lava));
        }
        if (cLayer == 13) // 13 is Environment
        {
            StartCoroutine(CrashCoroutine(CrashSeverity.Heavy));
        }
    }

    private IEnumerator CrashCoroutine(CrashSeverity crashSeverity)
    {
        crashed = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        switch (crashSeverity)
        {
            case CrashSeverity.Light:
                Debug.Log("Crash light");
                dialogueUI.ShowMessage(DialogueUI.DialogueMessageType.LightCrash);
                break;
            case CrashSeverity.Heavy:
                Debug.Log("Crash heavy");
                dialogueUI.ShowMessage(DialogueUI.DialogueMessageType.HeavyCrash);
                break;
            case CrashSeverity.Major:
                Debug.Log("Crash major");
                dialogueUI.ShowMessage(DialogueUI.DialogueMessageType.HeavyCrash);
                break;
            case CrashSeverity.Lava:
                Debug.Log("Crash lava");
                dialogueUI.ShowMessage(DialogueUI.DialogueMessageType.LavaCrash);
                break;
            default:
                Debug.Log("Crash type unknown");
                break;
        }

        yield return new WaitForSeconds(2.0f); // This will be based on time to complete anims etc.

        crashed = false;

        spawner.SpawnShip();
    }
}
