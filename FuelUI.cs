using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    private Fuel playerFuel;

    [SerializeField] private Image[] fuelImages;

    // TODO - variables for sprites

    private void Start()
    {
        playerFuel = FindObjectOfType<Fuel>();
    }

    private void Update()
    {
        UpdateFuelUI();
    }

    private void UpdateFuelUI()
    {
        int currentFuel = playerFuel.CurrentFuel;

        for (int i = 0; i < fuelImages.Length; i++)
        {
            if (i < currentFuel)
            {
                fuelImages[i].color = Color.white;
            }
            else
            {
                fuelImages[i].color = Color.gray;
            }
        }
    }
}
