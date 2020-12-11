using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] RankingsScriptableObject rankingsData;
    LandingHandler lHandler;
    Fuel fuel;

    private void Awake()
    {
        lHandler = GetComponent<LandingHandler>();
        fuel = GetComponent<Fuel>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (lHandler.Landed && collision.gameObject.tag == "LandingPad")
        {
            LandingPad lp = collision.gameObject.GetComponentInParent<LandingPad>();

            if (!lp.IsStartingPad && !lp.IsCleared)
            {
                lp.SetAsCleared();

                int fuelUsed = fuel.GetFuelUsedSinceLastCheckpoint();
                float landingVelocity = lHandler.LandingVelocity;
                float landingAngle = lHandler.LandingAngle;
                int rank = CalculateRank(landingVelocity, landingAngle, fuelUsed, lp.FuelTarget);
                //rankingsData.rankings[lp.StageID] = rank;
                rankingsData.SetRanking(lp.StageID, rank);

                FindObjectOfType<UIController>().ShowStageClearedUI(rank);
                fuel.ResetFuelUsedSinceLastCheckPoint();
            }
        }
    }

    private int CalculateRank(float velocity, float angle, int fuelUsed, int fuelTarget)
    {
        int velocityRank;
        int angleRank;
        int fuelRank;

        if (velocity < 0.5f)
        {
            velocityRank = 3;
        }
        else if (velocity < 1f)
        {
            velocityRank = 2;
        }
        else
        {
            velocityRank = 1;
        }

        if (angle < 5)
        {
            angleRank = 3;
        }
        else if (angle < 10)
        {
            angleRank = 2;
        }
        else
        {
            angleRank = 1;
        }

        if (fuelUsed <= fuelTarget)
        {
            fuelRank = 3;
        }
        else if (fuelUsed <= fuelTarget + 1)
        {
            fuelRank = 2;
        }
        else
        {
            fuelRank = 1;
        }

        int avg = Mathf.RoundToInt((velocityRank + angleRank + fuelRank)/3);

        return avg;
    }
}
