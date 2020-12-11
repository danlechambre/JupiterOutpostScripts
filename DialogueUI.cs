using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    // private DialogueSFX sfx;
    private AudioManager aManager;
    [SerializeField] private Animator copilotAnim;

    [SerializeField] private Text dialogueText;

    [SerializeField] private string[] levelStartDialogues;
    [SerializeField] private string[] checkpointClearedDialogues;
    [SerializeField] private string[] levelClearedDialogues;
    [SerializeField] private string[] lightCrashDialogues;
    [SerializeField] private string[] heavyCrashDialogues;
    [SerializeField] private string[] lavaCrashDialogues;
    [SerializeField] private string[] fuelEmptyDialogues;

    private bool messageDisplayInProgress;
    private float timeSinceLastMessage;

    [SerializeField] private float timeToWait;
    [SerializeField] private float messageDisplayTime;
    [SerializeField] private float fadeTime;

    public enum DialogueMessageType
    {
        LevelStart = 0,
        CheckpointCleared = 1,
        LevelCleared = 2,
        LightCrash = 3,
        HeavyCrash = 4,
        LavaCrash = 5,
        FuelEmpty = 6
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        aManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = 0;
        }

        messageDisplayInProgress = false;

        copilotAnim.SetFloat("Speed", 0f);
    }

    private void Update()
    {
        if (!messageDisplayInProgress)
        {
            timeSinceLastMessage += Time.deltaTime;
        }
    }

    public void ShowMessage(DialogueMessageType messageType)
    {
        bool displayMessage = Random.value > 0.5f;

        if (timeSinceLastMessage < timeToWait || !displayMessage)
        {
            return;
        }

        string message;

        switch (messageType)
        {
            case DialogueMessageType.LevelStart:
                message = levelStartDialogues[Random.Range(0, levelStartDialogues.Length)];
                break;
            case DialogueMessageType.CheckpointCleared:
                message = checkpointClearedDialogues[Random.Range(0, checkpointClearedDialogues.Length)];
                break;
            case DialogueMessageType.LevelCleared:
                message = levelClearedDialogues[Random.Range(0, levelClearedDialogues.Length)];
                break;
            case DialogueMessageType.LightCrash:
                message = lightCrashDialogues[Random.Range(0, levelClearedDialogues.Length)];
                break;
            case DialogueMessageType.HeavyCrash:
                message = heavyCrashDialogues[Random.Range(0, heavyCrashDialogues.Length)];
                break;
            case DialogueMessageType.LavaCrash:
                message = lavaCrashDialogues[Random.Range(0, lavaCrashDialogues.Length)];
                break;
            case DialogueMessageType.FuelEmpty:
                message = fuelEmptyDialogues[Random.Range(0, fuelEmptyDialogues.Length)];
                break;
            default:
                message = "Default message!";
                break;
        }

        IEnumerator showMessageCoroutine = ShowMessageCoroutine(message);
        StartCoroutine(showMessageCoroutine);
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        timeSinceLastMessage = 0f;
        messageDisplayInProgress = true;
        copilotAnim.SetFloat("Speed", 1f);

        dialogueText.text = message;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeTime * Time.unscaledDeltaTime;
            yield return null;
        }

        aManager.PlayDialogueSFX();
        yield return new WaitForSeconds(messageDisplayTime);


        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeTime * Time.unscaledDeltaTime;
            yield return null;
        }


        messageDisplayInProgress = false;
        copilotAnim.SetFloat("Speed", 0f);
    }
}
