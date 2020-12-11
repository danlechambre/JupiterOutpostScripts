using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private PlayerSettings playerSettings;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private AudioClip[] levelSFX;
    [SerializeField] private AudioClip[] dialogueSFX;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource levelSFXPlayer;
    [SerializeField] private AudioSource dialogueSFXPlayer;

    [SerializeField] private float mixerSnapshotTransitionTime;

    private AudioMixerSnapshot mutedSnapshot;
    private AudioMixerSnapshot defaultSnapshot;
    private AudioMixerSnapshot dialoguePlayingSnaphsot;

    string currentScene;

    private void Awake()
    {
        playerSettings = FindObjectOfType<PlayerSettings>();
        mutedSnapshot = mixer.FindSnapshot("AudioMuted");
        defaultSnapshot = mixer.FindSnapshot("Default");
        dialoguePlayingSnaphsot = mixer.FindSnapshot("DialoguePlaying");
    }

    private void Start()
    {
        SetVolumeFromPreferences();

        currentScene = this.gameObject.scene.name;

        FadeInMaster();
        PlayLevelMusic();
    }

    private void FadeInMaster()
    {
        defaultSnapshot.TransitionTo(1f);
    }

    public void PlayLevelMusic()
    {
        int trackToPlay;

        switch (currentScene)
        {
            case "World_1":
                trackToPlay = 0;
                break;
            case "World_2":
                trackToPlay = 1;
                break;
            case "World_3":
                trackToPlay = 2;
                break;
            case "World_4":
                trackToPlay = 3;
                break;
            case "World_5":
                trackToPlay = 4;
                break;
            default:
                trackToPlay = 0;
                break;
        }

        musicPlayer.clip = musicTracks[trackToPlay];
        musicPlayer.Play();
    }

    public void PlayLevelSFX()
    {

    }

    public void PlayDialogueSFX()
    {
        dialoguePlayingSnaphsot.TransitionTo(0.25f);
        
        int clipToPlay = UnityEngine.Random.Range(0, dialogueSFX.Length);
        dialogueSFXPlayer.clip = dialogueSFX[clipToPlay];
        dialogueSFXPlayer.Play();

        float clipLength = dialogueSFXPlayer.clip.length;
        Debug.Log(clipLength);
        IEnumerator c = SnapshotAutoTransition(clipLength, defaultSnapshot);
        StartCoroutine(c);
    }

    IEnumerator SnapshotAutoTransition(float timeToWait, AudioMixerSnapshot destinationSnapshot)
    {
        yield return new WaitForSeconds(timeToWait);
        destinationSnapshot.TransitionTo(0.25f);
    }

    public void SetVolumeFromPreferences()
    {
        levelSFXPlayer.volume = playerSettings.SfxVolumePref;
        musicPlayer.volume = playerSettings.MusicVolumePref;
    }

    public void ChangeSfxVolume(float newValue)
    {
        levelSFXPlayer.volume = newValue;
        playerSettings.SfxVolumePref = newValue;
    }

    public void ChangeMusicVolume(float newValue)
    {
        musicPlayer.volume = newValue;
        playerSettings.MusicVolumePref = newValue;
    }
}
