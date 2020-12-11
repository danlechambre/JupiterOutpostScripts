using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingHandler : MonoBehaviour
{
    CrashHandler cHandler;

    [SerializeField] private float landingTimer;

    private float landedTimeElapsed;
    private bool landed;
    private float landingVelocity;
    private float landingAngle;

    public bool Landed { get { return landed; } }

    public float LandingVelocity { get { return landingVelocity; } }
    public float LandingAngle { get { return landingAngle; } }

    private void Awake()
    {
        cHandler = GetComponent<CrashHandler>();
    }

    private void Start()
    {
        landedTimeElapsed = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        landingVelocity = collision.relativeVelocity.sqrMagnitude;
        landingAngle = Quaternion.Angle(transform.rotation, Quaternion.identity);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != 12) // 12 = friendly
        {
            return;
        }

        Quaternion shipRotation = transform.rotation;
        float angle = Quaternion.Angle(shipRotation, Quaternion.identity);

        bool shipStraight = Mathf.Approximately(angle, 0f);

        if (shipStraight && !cHandler.Crashed)
        {
            landedTimeElapsed += Time.deltaTime;
            if (landedTimeElapsed > landingTimer)
            {
                landed = true;
            }
        }
    }

    private void OnCollisionExit()
    {
        landed = false;
        landedTimeElapsed = 0f;
    }
}
