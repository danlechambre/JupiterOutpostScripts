using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private PlayerSettings playerSettings;
    private GameStatus gameStatus;

    [SerializeField] CanvasGroup titleScreen;
    [SerializeField] CanvasGroup gameUI;
    [SerializeField] CanvasGroup settingsUI;
    [SerializeField] CanvasGroup rankingsUI;
    [SerializeField] CanvasGroup lvlClearUI;
    [SerializeField] Button startGameBtn;
    [SerializeField] Button titleSettingsBtn;
    [SerializeField] Button inGameSettingsBtn;
    [SerializeField] Button exitSettingsBtn;
    [SerializeField] Button settingsRankingsBtn;
    [SerializeField] Button clearedRankingsBtn;
    [SerializeField] Button exitRankingsBtn;

    [SerializeField] Slider musicVolSlider;
    [SerializeField] Slider sfxVolSlider;

    [SerializeField] RankingsIcons rankingsIcons;
    [SerializeField] Image stageClearRankIcon;

    AudioSource[] audioSources;

    CanvasGroup lastUILoaded;
    bool gamePaused;

    [SerializeField] private float slowFadeTime;
    [SerializeField] private float quickFadeTime;
    [SerializeField] private float showOverlayTime;

    private void Awake()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();
        gameStatus = FindObjectOfType<GameStatus>();
        audioSources = FindObjectsOfType<AudioSource>();
    }

    private void OnEnable()
    {
        startGameBtn.onClick.AddListener(() => StartGameButton());
        titleSettingsBtn.onClick.AddListener(() => ShowSettings(false));
        inGameSettingsBtn.onClick.AddListener(() => ShowSettings(true));
        exitSettingsBtn.onClick.AddListener(() => ExitSettings());
        settingsRankingsBtn.onClick.AddListener(() => ShowRankingsUI());
        clearedRankingsBtn.onClick.AddListener(() => ShowRankingsUI());
        exitRankingsBtn.onClick.AddListener(() => ExitRankingsUI());
        musicVolSlider.onValueChanged.AddListener(delegate { MusicVolSlider(); });
        sfxVolSlider.onValueChanged.AddListener(delegate { SfxVolSlider(); });
    }

    private void Start()
    {
        titleScreen.alpha = 1f;
        titleScreen.interactable = true;

        gameUI.alpha = 0f;
        gameUI.interactable = false;
        gameUI.gameObject.SetActive(false);

        settingsUI.alpha = 0f;
        settingsUI.interactable = false;
        settingsUI.gameObject.SetActive(false);

        rankingsUI.alpha = 0f;
        rankingsUI.interactable = false;
        rankingsUI.gameObject.SetActive(false);

        lvlClearUI.alpha = 0f;
        lvlClearUI.interactable = false;
        lvlClearUI.gameObject.SetActive(false);

        gamePaused = false;

        musicVolSlider.value = playerSettings.MusicVolumePref;
        sfxVolSlider.value = playerSettings.SfxVolumePref;
    }

    public void StartGameButton()
    {
        IEnumerator c = FadeSwap(titleScreen, gameUI, quickFadeTime);
        StartCoroutine(c);

        gameStatus.StartGame();
    }

    public void ShowSettings(bool inGame)
    {
        CanvasGroup from;

        if (inGame)
        {
            from = gameUI;
            gamePaused = true;
            Time.timeScale = 0f;
            foreach (AudioSource source in audioSources)
            {
                if (source.name != "MusicPlayer")
                {
                    source.Pause();
                }
            }
        }
        else
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
            from = titleScreen;
        }

        IEnumerator c = FadeSwap(from, settingsUI, quickFadeTime);
        StartCoroutine(c);
    }

    public void ExitSettings()
    {
        CanvasGroup to;

        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1f;
            foreach (AudioSource source in audioSources)
            {
                source.UnPause();
            }
            to = gameUI;
        }
        else
        {
            Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
            to = titleScreen;
        }

        IEnumerator c = FadeSwap(settingsUI, to, quickFadeTime);
        StartCoroutine(c);
    }

    public void ShowRankingsUI()
    {
        rankingsIcons.RefreshRankings();
        IEnumerator c = FadeInOverlay(rankingsUI, quickFadeTime);
        StartCoroutine(c);
    }

    public void ExitRankingsUI()
    {
        IEnumerator c = FadeOutOverlay(rankingsUI, quickFadeTime);
        StartCoroutine(c);
    }

    public void ShowStageClearedUI(int rank)
    {
        if (rank == 1)
        {
            stageClearRankIcon.sprite = rankingsIcons.Rank1Img;
        }
        else if (rank == 2)
        {
            stageClearRankIcon.sprite = rankingsIcons.Rank2Img;
        }
        else if (rank == 3)
        {
            stageClearRankIcon.sprite = rankingsIcons.Rank3Img;
        }

        IEnumerator c = FadeInOverlayAutoFadeOut(lvlClearUI, slowFadeTime);
        StartCoroutine(c);
    }

    IEnumerator FadeInOverlay(CanvasGroup canvasGroup, float fadeSpeed)
    {
        canvasGroup.gameObject.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.interactable = true;
    }

    IEnumerator FadeOutOverlay(CanvasGroup canvasGroup, float fadeSpeed)
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.gameObject.SetActive(false);
    }

    IEnumerator FadeInOverlayAutoFadeOut(CanvasGroup canvasGroup, float fadeSpeed)
    {
        canvasGroup.gameObject.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.interactable = true;

        yield return new WaitForSeconds(showOverlayTime);

        canvasGroup.interactable = false;

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.gameObject.SetActive(false);
    }

    IEnumerator FadeSwap(CanvasGroup from, CanvasGroup to, float fadeSpeed)
    {
        to.gameObject.SetActive(true);
        from.interactable = false;

        while (from.alpha > 0f || to.alpha < 1f)
        {
            from.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            to.alpha += fadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        from.gameObject.SetActive(false);
        to.interactable = true;
    }

    IEnumerator FadeInUICanvasGroup(CanvasGroup fadeFrom, CanvasGroup fadeTo, float fadeSpeed)
    {
        fadeTo.interactable = true;
        fadeFrom.interactable = false;

        fadeTo.gameObject.SetActive(true);

        while (fadeTo.alpha < 1f && fadeFrom.alpha > 0f)
        {
            fadeTo.alpha += fadeSpeed * Time.deltaTime;
            fadeFrom.alpha -= fadeSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        fadeFrom.gameObject.SetActive(false);
    }

    private void MusicVolSlider()
    {
        GameObject.FindObjectOfType<AudioManager>().ChangeMusicVolume(musicVolSlider.value);
    }

    private void SfxVolSlider()
    {
        GameObject.FindObjectOfType<AudioManager>().ChangeSfxVolume(sfxVolSlider.value);
    }

    private void OnDisable()
    {
        startGameBtn.onClick.RemoveAllListeners();
        titleSettingsBtn.onClick.RemoveAllListeners();
        inGameSettingsBtn.onClick.RemoveAllListeners();
        exitSettingsBtn.onClick.RemoveAllListeners();
        settingsRankingsBtn.onClick.RemoveAllListeners();
        clearedRankingsBtn.onClick.RemoveAllListeners();
        exitRankingsBtn.onClick.RemoveAllListeners();
        musicVolSlider.onValueChanged.RemoveAllListeners();
        sfxVolSlider.onValueChanged.RemoveAllListeners();
    }
}