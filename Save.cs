using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public bool[] WorldsUnlocked = new bool[5];
    public int[] Rankings = new int[25];
    public bool GameCompleted;
}
