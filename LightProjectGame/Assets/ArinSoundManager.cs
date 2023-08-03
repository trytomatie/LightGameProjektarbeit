using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArinSoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public string[] subtitles;

    public static ArinSoundManager instance;
    public void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
