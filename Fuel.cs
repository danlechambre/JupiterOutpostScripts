using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    private ShipController shipController;
    private LandingHandler lHandler;
    private DialogueUI dialogueUI;

    private Material shipMaterial;

    private int currentFuel;
    private float refuelTimerElapsed = 0.0f;
    private float depleteFuelTimerElapsed = 0.0f;
    private int fuelUsedSinceLastCheckPoint;

    [SerializeField] int maxFuel = 100;
    [SerializeField] private int refuelAmount = 10;
    [SerializeField] private float refuelTimer = 0.5f;
    [SerializeField] private int depleteFuelAmount = 1;
    [SerializeField] private float depleteFuelTimer = 0.5f;
    [SerializeField] private Texture[] eMaps;

    public bool FuelEmpty
    {
        get
        {
            return currentFuel <= 0;
        }
    }

    public int CurrentFuel
    {
        get { return currentFuel; }
    }

    public int GetFuelUsedSinceLastCheckpoint()
    {
        return fuelUsedSinceLastCheckPoint;
    }

    public void ResetFuelUsedSinceLastCheckPoint()
    {
        fuelUsedSinceLastCheckPoint = 0;
    }

    private void Awake()
    {
        shipController = GetComponent<ShipController>();
        lHandler = GetComponent<LandingHandler>();
        dialogueUI = GameObject.FindObjectOfType<DialogueUI>();

        shipMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void Start()
    {
        currentFuel = maxFuel;
        UpdateFuelLight(currentFuel);
    }

    private void Update()
    {
        if (shipController.Thrusting)
        {
            DepleteFuel();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 12 && lHandler.Landed) // Friendly
        {
            Refuel();
        }
    }

    private void DepleteFuel()
    {
        depleteFuelTimerElapsed += Time.deltaTime;

        if (depleteFuelTimerElapsed > depleteFuelTimer)
        {
            currentFuel -= depleteFuelAmount;
            fuelUsedSinceLastCheckPoint += depleteFuelAmount;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            UpdateFuelLight(currentFuel);
            depleteFuelTimerElapsed = 0.0f;

            if (currentFuel == 0)
            {
                dialogueUI.ShowMessage(DialogueUI.DialogueMessageType.FuelEmpty);
            }
        }
    }

    private void Refuel()
    {
        depleteFuelTimerElapsed = 0f;
        refuelTimerElapsed += Time.deltaTime;
        if (refuelTimerElapsed > refuelTimer)
        {
            currentFuel += refuelAmount;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            refuelTimerElapsed = 0.0f;
            UpdateFuelLight(currentFuel);
        }
    }

    private void UpdateFuelLight(int currentFuel)
    {
        
        shipMaterial.SetTexture("_EmissionMap", eMaps[currentFuel]);
    }
}