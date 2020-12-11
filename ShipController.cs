using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody rb;
    private Fuel fuel;
    private CrashHandler crashHandler;
    private CameraController camController;
    private GameStatus gameStatus;
    private ShipSFXController shipSFXController;

    // Config
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float sqrMaxVelocity = 50.0f;

    // State
    private bool thrusting = false;
    private bool flyingTooHigh = false;

    private bool ShipOperable;

    public bool Thrusting
    {
        get { return this.thrusting; }
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        fuel = GetComponent<Fuel>();
        crashHandler = GetComponent<CrashHandler>();
        camController = FindObjectOfType<CameraController>();
        gameStatus = FindObjectOfType<GameStatus>();
        shipSFXController = GetComponent<ShipSFXController>();
    }

    private void OnEnable()
    {
        playerControls.ShipControls.Thrust.Enable();
        playerControls.ShipControls.Thrust.started += CheckThrust;
        playerControls.ShipControls.Thrust.canceled += CheckThrust;

        playerControls.ShipControls.Rotate.Enable();
    }

    private void OnDisable()
    {
        playerControls.ShipControls.Thrust.Disable();
        playerControls.ShipControls.Thrust.started -= CheckThrust;
        playerControls.ShipControls.Thrust.canceled -= CheckThrust;

        playerControls.ShipControls.Rotate.Disable();
    }

    private void FixedUpdate()
    {
        if (ShipOperable)
        {
            RespondToThrust();
            RespondToRotation();
        }
    }

    private void Update()
    {
        // another condition check needed for start sequence
        ShipOperable = gameStatus.GameStarted && !fuel.FuelEmpty && !crashHandler.Crashed && !camController.IsRefreshing;
    }

    private void CheckThrust(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            thrusting = true;
        }
        else if (callbackContext.canceled)
        {
            thrusting = false;
        }
    }

    private void RespondToThrust()
    {
        if (thrusting && rb.velocity.sqrMagnitude < sqrMaxVelocity && !flyingTooHigh)
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            shipSFXController.StartMainThrustFX();
        }
        else
        {
            shipSFXController.StopMainThrustFX();
        }
    }

    private void RespondToRotation()
    {
        InputAction action = playerControls.ShipControls.Rotate;
        if (action.ReadValue<float>() != 0)
        {
            float torque = action.ReadValue<float>() * rcsThrust;

            rb.AddRelativeTorque(0, 0, torque);
            shipSFXController.StartSideThrustFX(torque);
        }
        else
        {
            shipSFXController.StopSideThrust();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Layer 10 = AltitudeBoundary
        {
            flyingTooHigh = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) // Layer 10 = AltitudeBoundary
        {
            flyingTooHigh = false;
        }
    }
}