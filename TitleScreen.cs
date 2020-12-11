using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour, IPointerClickHandler
{
    private GameStatus gameStatus;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameStatus.StartGame();
    }
}
