using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    private bool gameStarted;

    public bool GameStarted { get { return gameStarted; } }

    public void StartGame()
    {
        gameStarted = true;
        FindObjectOfType<CameraController>().RefreshCamera();
    }

    public void EndGame()
    {
        gameStarted = false;
    }
}
