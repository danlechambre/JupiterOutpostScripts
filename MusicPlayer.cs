using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        mixer.FindSnapshot("TestSnapshot").TransitionTo(3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
