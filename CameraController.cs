using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameStatus gameStatus;
    private GameObject target;
    private CheckpointController cpController;
    
    Rigidbody targetRb;
    LandingHandler targetLHandler;
    Camera cam;

    // Config -- change values in inspector
    [Header("Zoom Config")]
    [SerializeField] private float zoomInSpeed = 0.3f;
    [SerializeField] private float zoomOutSpeed = 0.3f;
    [SerializeField] private float zoomNear = -15f;
    [SerializeField] private float zoomFar = -30f;

    [Header("Pan Config")]
    [SerializeField] float slowPan = 0.1f;
    [SerializeField] float quickPan = 1.0f;
    [SerializeField] float panOffsetScale = 1.0f;
    [SerializeField] float panSpeedLerpIncrease = 0.3f;
    [SerializeField] float panSpeedLerpDecrease = 1.0f;

    [Header("Other")]
    [SerializeField] float hoverTimer = 2.0f;
    [SerializeField] private float velTrigger = 12.0f;
    [SerializeField] float cameraConstraintOffset = 5.0f;
    [SerializeField] float frustumPadding = 5.0f;
    [SerializeField] float camStartupTime = 2.0f;
    [SerializeField] float camRefreshPositionSpeed = 10.0f;
    [SerializeField] float camRefreshRotationSpeed = 20.0f;

    float panSpeed = 0.0f;
    float targetVelSqr = 0f;
    Vector3 rbVelocity = Vector3.zero;
    float hoverTimeElapsed = 0f;
    bool hovering = false;
    float constraintLeft;
    float constraintRight;
    IEnumerator camRefreshCoroutine;

    public bool IsRefreshing { get; private set; }

    public void SetFrustrumPadding(float padding)
    {
        frustumPadding += padding;
    }

    private void Awake()
    {
        gameStatus = FindObjectOfType<GameStatus>();
        target = GameObject.Find("PlayerShip");
        cpController = FindObjectOfType<CheckpointController>();
        targetRb = target.GetComponent<Rigidbody>();
        targetLHandler = target.GetComponent<LandingHandler>();

        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        rbVelocity = targetRb.velocity;
        targetVelSqr = targetRb.velocity.sqrMagnitude;
    }

    private void Update()
    {
        CheckHovering();
    }

    private void LateUpdate()
    {
        if (!IsRefreshing && gameStatus.GameStarted)
        {
            CalculateZoom();
            CalculatePan();
        }
    }

    private void CheckHovering()
    {
        if (targetVelSqr < velTrigger && !targetLHandler.Landed)
        {
            hoverTimeElapsed += Time.deltaTime;
        }
        else
        {
            hoverTimeElapsed = 0f;
        }

        if (hoverTimeElapsed > hoverTimer)
        {
            hovering = true;
        }
        else
        {
            hovering = false;
        }
    }

    public void SetCameraConstraints(LandingPad leftPad, LandingPad rightPad)
    {
        constraintLeft = leftPad.transform.position.x + cameraConstraintOffset;
        constraintRight = rightPad.transform.position.x - cameraConstraintOffset;
    }

    private void CalculatePan()
    {
        if (targetVelSqr > velTrigger)
        {
            panSpeed = Mathf.Lerp(panSpeed, quickPan, panSpeedLerpIncrease * Time.deltaTime);
        }
        else
        {
            panSpeed = Mathf.Lerp(panSpeed, slowPan, panSpeedLerpDecrease * Time.deltaTime);
        }

        float newXPos;
        float newYPos;
        float xOffset = rbVelocity.x * panOffsetScale;
        float yOffset = rbVelocity.y * panOffsetScale;

        float clampedTargetX = Mathf.Clamp(target.transform.position.x + xOffset, constraintLeft, constraintRight);

        newXPos = Mathf.Lerp(transform.position.x, clampedTargetX, panSpeed * Time.deltaTime);
        newYPos = Mathf.Lerp(transform.position.y, target.transform.position.y + yOffset, panSpeed * Time.deltaTime);

        this.transform.position = new Vector3(newXPos, newYPos, transform.position.z);
    }

    private void CalculateZoom()
    {
        float currentCamZ = transform.position.z;
        float newCamZ;

        if (hovering)
        {
            newCamZ = Mathf.Lerp(currentCamZ, zoomNear, zoomInSpeed * Time.deltaTime);
        }
        else
        {
            newCamZ = Mathf.Lerp(currentCamZ, zoomFar, zoomOutSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, newCamZ);
    }

    public void RefreshCamera()
    {
        if (!gameStatus.GameStarted)
        {
            return;
        }

        if (camRefreshCoroutine != null)
        {
            StopCoroutine(camRefreshCoroutine);
        }

        if (camRefreshCoroutine == null)
        {
            camRefreshCoroutine = RefreshCameraPos();
            // camRefreshCoroutine = SetCameraStartingPos();
        }
        else
        {
            camRefreshCoroutine = RefreshCameraPos();
        }

        StartCoroutine(camRefreshCoroutine);
    }

    private IEnumerator SetCameraStartingPos()
    {
        IsRefreshing = true;

        Vector3 newCamPos = CalculateViewForActiveCheckpoints();

        transform.SetPositionAndRotation(newCamPos, Quaternion.identity);

        yield return new WaitForSeconds(camStartupTime);

        if (IsRefreshing)
        {
            IsRefreshing = false;
        }
    }

    private IEnumerator RefreshCameraPos()
    {
        IsRefreshing = true;

        Vector3 targetCamPos = CalculateViewForActiveCheckpoints();
        Vector3 camVelocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetCamPos) > Mathf.Epsilon || Vector3.Distance(transform.rotation.eulerAngles, Quaternion.identity.eulerAngles) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetCamPos, camRefreshPositionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, camRefreshPositionSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(camStartupTime);
        if (cam.farClipPlane != 1000f)
        {
            cam.farClipPlane = 1000f;
        }

        if (IsRefreshing)
        {
            IsRefreshing = false;
        }
    }

    private Vector3 CalculateViewForActiveCheckpoints()
    {
        Camera cam = GetComponent<Camera>();

        Vector2 pointA = cpController.CurrentLandingPad.transform.position;
        Vector2 pointB = cpController.NextLandingPad.transform.position;
        Vector2 midpoint = new Vector2((pointA.x + pointB.x) / 2, (pointA.y + pointB.y) / 2);

        float d = Vector2.Distance(pointA, pointB);

        var frustumWidth = (pointB.x - pointA.x) + frustumPadding;
        var frustumHeight = frustumWidth / cam.aspect;
        var distance = frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        Vector3 newCamPos = new Vector3(midpoint.x, midpoint.y, -distance);
        return newCamPos;
    }
}