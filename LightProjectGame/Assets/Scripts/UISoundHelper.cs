using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Geschrieben von mir Markus also wenn schlecht ist Chris wars nicht
/// ein Script um UI sounds zu verleihen
/// </summary>
public class UISoundHelper : MonoBehaviour
{
    public AudioClip hoverClip;
    public AudioClip clickClip;


    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverClip);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickClip);

    }
}
