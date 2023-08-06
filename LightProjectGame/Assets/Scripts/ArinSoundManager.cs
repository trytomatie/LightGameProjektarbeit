using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArinSoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    public AudioClip drinkPotion;

    public string[] subtitles;
    public AudioClip[] nariGiggle;
    public AudioClip[] arinAttack;
    public AudioClip[] arinTakeDamage;
    public AudioClip[] arinJump;
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
