using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraColliders : MonoBehaviour
{
    private Camera cam;
    private CameraController cameraController;

    private float currentScreenHeight;
    private float leftX;
    private float xValLeftSideOfScreen;
    private float xValRightSideOfScreen;
    private GameObject leftBarrier;
    private GameObject rightBarrier;
    private GameObject barrierParent;
    private bool barriersActive;

    [SerializeField] float barrierOffset = 0.0f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cameraController = GetComponent<CameraController>();
        CreateBarriers();
    }

    private void FixedUpdate()
    {
        SetBarrierTransforms();
    }

    private void Update()
    {
        if (cameraController.IsRefreshing && barriersActive)
        {
            barrierParent.SetActive(false);
            barriersActive = false;
        }

        if (!cameraController.IsRefreshing && !barriersActive)
        {
            barrierParent.SetActive(true);
            barriersActive = true;
        }
    }

    private void CreateBarriers()
    {
        barrierParent = new GameObject("BarrierParent");
        barrierParent.layer = 9;

        leftBarrier = new GameObject("LeftBarrier", typeof(BoxCollider));
        leftBarrier.transform.SetParent(barrierParent.transform);
        leftBarrier.layer = 9;

        rightBarrier = new GameObject("RightBarrier", typeof(BoxCollider));
        rightBarrier.transform.SetParent(barrierParent.transform);
        rightBarrier.layer = 9;

        SetBarrierTransforms();
    }

    private void SetBarrierTransforms()
    {
        float zOffset = -transform.position.z;
        Vector3 vpBottomLeftInWorld = cam.ViewportToWorldPoint(new Vector3(0f, 0f, zOffset));
        Vector3 vpTopRightInWorld = cam.ViewportToWorldPoint(new Vector3(1f, 1f, zOffset));
        Vector3 vpCenterPointInWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, zOffset));

        float vpHeightInWorldUnits = vpTopRightInWorld.y - vpBottomLeftInWorld.y;

        Vector3 newScale = new Vector3(1f, vpHeightInWorldUnits, 1f);

        leftBarrier.transform.localScale = newScale;
        rightBarrier.transform.localScale = newScale;

        Vector3 newLeftPos = new Vector3(vpBottomLeftInWorld.x - barrierOffset, vpCenterPointInWorld.y, 0f);
        leftBarrier.transform.position = newLeftPos;

        Vector3 newRightPos = new Vector3(vpTopRightInWorld.x + barrierOffset, vpCenterPointInWorld.y, 0f);
        rightBarrier.transform.position = newRightPos;
    }
}