using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private LandingPad[] landingPads;

    private LandingPad currentLandingPad;
    private LandingPad nextLandingPad;

    private void Awake()
    {
        
    }

    private void Start()
    {
        CurrentLandingPad = landingPads[0];

        for (int i = 0; i < landingPads.Length; i++)
        {
            landingPads[i].Id = i;
        }
    }

    public LandingPad CurrentLandingPad
    {
        get { return currentLandingPad; }
        set
        {
            currentLandingPad = landingPads[value.Id];
            if (!value.IsFinalLandingPad)
            {
                NextLandingPad = landingPads[value.Id + 1];
                CameraController cc = FindObjectOfType<CameraController>();
                cc.SetCameraConstraints(CurrentLandingPad, NextLandingPad);
                cc.RefreshCamera();
            }
            else
            {
                SceneController sc = FindObjectOfType<SceneController>();
                sc.LoadNextWorld(5f);
            }
        }
    }

    public LandingPad NextLandingPad
    {
        get { return nextLandingPad; }
        private set
        {
            nextLandingPad = value;
        }
    }
}
