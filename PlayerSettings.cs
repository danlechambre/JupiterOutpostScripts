using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    private float musicVolumePref;
    private float sfxVolumePref;

    public float MusicVolumePref
    {
        get
        {
            return PlayerPrefs.GetFloat("music-volume", 1f);
        }
        set
        {
            PlayerPrefs.SetFloat("music-volume", value);
            PlayerPrefs.Save();
        }
    }

    public float SfxVolumePref
    {
        get
        {
            return PlayerPrefs.GetFloat("sfx-volume", 1f);
        }
        set
        {
            PlayerPrefs.SetFloat("sfx-volume", value);
            PlayerPrefs.Save();
        }
    }
}
