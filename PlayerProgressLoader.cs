using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressLoader : MonoBehaviour
{
    [SerializeField] RankingsScriptableObject rankings;

    private void Awake()
    {
        rankings.LoadGame();
    }
}
