using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private RankingsScriptableObject rankingsData;
    private CanvasGroup sceneTransitionUI;

    private string currentScene;
    private bool isLastWorld;

    [SerializeField] float fadeSpeed;

    public string CurrentScene { get { return currentScene; } }

    private void Awake()
    {
        currentScene = this.gameObject.scene.name;
        SetWorldToUnlocked();

        isLastWorld = currentScene == "World_5";   
    }

    private void SetWorldToUnlocked()
    {
        switch (currentScene)
        {
            case "World_1":
                rankingsData.SetWorldToUnlocked(0);
                break;
            case "World_2":
                rankingsData.SetWorldToUnlocked(1);
                break;
            case "World_3":
                rankingsData.SetWorldToUnlocked(2);
                break;
            case "World_4":
                rankingsData.SetWorldToUnlocked(3);
                break;
            case "World_5":
                rankingsData.SetWorldToUnlocked(4);
                break;
            default:
                Debug.LogError($"Can't unlock {currentScene}; {currentScene} is unknown");
                break;
        }
    }

    private void Start()
    {
        sceneTransitionUI = GameObject.Find("SceneTransition").GetComponent<CanvasGroup>();
        sceneTransitionUI.alpha = 1.0f;
        FadeIn();
    }

    private void FadeIn()
    {
        IEnumerator c = FadeInCoroutine();
        StartCoroutine(c);
    }

    private IEnumerator FadeInCoroutine()
    {
        while (sceneTransitionUI.alpha > 0f)
        {
            sceneTransitionUI.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void LoadWorld(string sceneToLoad)
    {
        IEnumerator c = SceneTransition(sceneToLoad, 0f);
        StartCoroutine(c);
    }

    public void LoadWorld(string sceneToLoad, float delay)
    {
        IEnumerator c = SceneTransition(sceneToLoad, delay);
        StartCoroutine(c);
    }

    public void LoadNextWorld(float delay)
    {
        int indexToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (!isLastWorld)
        {
            string sceneToLoad = "World_" + (indexToLoad + 1);
            IEnumerator c = SceneTransition(sceneToLoad, delay);
            StartCoroutine(c);
        }
        else
        {
            if (rankingsData.gameCompleted == false)
            {
                rankingsData.SetGameCompleted();
                IEnumerator c = SceneTransition("Credits", delay);
                StartCoroutine(c);
            }
            else
            {
                IEnumerator c = SceneTransition("World_1", delay);
                StartCoroutine(c);
            }
        }
    }

    private IEnumerator SceneTransition(string sceneToLoad, float delay)
    {
        yield return new WaitForSeconds(delay);

        while (sceneTransitionUI.alpha < 1f)
        {
            sceneTransitionUI.alpha += fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
