using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] dialogueAudioClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDialogueSFX()
    {
        audioSource.clip = dialogueAudioClips[Random.Range(0, dialogueAudioClips.Length)];
        audioSource.Play();
    }
}
