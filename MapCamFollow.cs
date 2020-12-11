using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamFollow : MonoBehaviour
{
    GameObject target;
    [SerializeField] float xOffset;

    private void Awake()
    {
        target = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        Vector3 targetPos = target.transform.position;
        this.transform.position = new Vector3(targetPos.x + xOffset, targetPos.y, this.transform.position.z);
    }
}
