using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    private CheckpointController cpController;

    [SerializeField] private bool startingPad = false;
    [SerializeField] private bool finalLandingPad = false;
    [SerializeField] private float frustumPaddingAdjustment = 0f;

    private int id;
    private bool checkpointCleared;
    private float landedTimeElapsed;

    [SerializeField] private int stageID;

    [SerializeField] private int fuelTarget = 3;
    
    public int FuelTarget { get { return fuelTarget; } }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int StageID { get { return stageID; } }

    public bool IsStartingPad { get { return startingPad; } }

    public bool IsFinalLandingPad { get { return finalLandingPad; } }

    public bool IsCleared { get { return checkpointCleared; } }

    private void Start()
    {
        cpController = GameObject.FindObjectOfType<CheckpointController>();

        checkpointCleared = false;
    }

    public void SetAsCleared()
    {
        FindObjectOfType<CameraController>().SetFrustrumPadding(frustumPaddingAdjustment);

        checkpointCleared = true;
        cpController.CurrentLandingPad = this;
    }
}