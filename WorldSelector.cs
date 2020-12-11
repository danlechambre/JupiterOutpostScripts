using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSelector : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RankingsScriptableObject rankingsData;

    private SceneController sController;

    private GameObject[] selectors;
    [SerializeField] private Vector3[] points;
    [SerializeField] private float[] scales;

    private void Awake()
    {
        sController = GameObject.FindObjectOfType<SceneController>();
    }

    private void Start()
    {
        selectors = new GameObject[this.transform.childCount];

        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i] = this.transform.GetChild(i).gameObject;
        }

        List<GameObject> activeSelectors = new List<GameObject>();

        for (int i = 0; i < selectors.Length; i++)
        {
            GameObject go = selectors[i];
            bool isCurrentWorld = go.name == sController.CurrentScene;
            bool worldUnlocked = rankingsData.worldsUnlocked[i];
            if (isCurrentWorld || !worldUnlocked)
            {
                go.SetActive(false);
            }
            else
            {
                activeSelectors.Add(go);
                Debug.Log($"{go.name} was added to activeSelectors ");
            }
        }

        for (int i = 0; i < activeSelectors.Count; i++)
        {
            activeSelectors[i].transform.position = points[i];
            float scale = scales[i];
            activeSelectors[i].transform.localScale = new Vector3(scale, scale, scale);
        }

        //int i = 0;
        //foreach (GameObject go in selectors)
        //{
        //    if (go.name == currentScene)
        //    {
        //        go.SetActive(false);
        //    }
        //    else
        //    {
        //        if (i < points.Length)
        //        {
        //            go.transform.position = points[i];
        //            float scale = scales[i];
        //            go.transform.localScale = new Vector3(scale, scale, scale);
        //            i++;
        //        }
        //        else
        //        {
        //            Debug.LogError("Index out of range for Selectors into Points");
        //        }
        //    }
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        string selectorName = eventData.pointerCurrentRaycast.gameObject.name;
        sController.LoadWorld(selectorName);
    }
}
