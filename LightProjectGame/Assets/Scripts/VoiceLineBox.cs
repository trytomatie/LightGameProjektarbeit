using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoiceLineBox : MonoBehaviour
{
    public bool singleTrigger = true;
    private bool triggered = false;
    public string subtitles;
    public AudioClip soundFile;
    private float disableTime = 0;
    public static float staticDisableTime = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (singleTrigger && triggered)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            GameManager.instance.subtitle.text = subtitles;
            GameManager.instance.subtitle.gameObject.SetActive(true);
            FeedbackManager.PlayVoiceLine(soundFile);
            disableTime = Time.time + soundFile.length + 1;
            staticDisableTime = Time.time + soundFile.length + 1;
            Invoke("HideSubtitles", soundFile.length);
            triggered = true;
        }
    }


    private void HideSubtitles()
    {
        if(disableTime == staticDisableTime)
        {
            GameManager.instance.subtitle.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Collider col = GetComponent<BoxCollider>();
        Gizmos.color = new Color(1, 0.5f, 0.3f, 0.2f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        Gizmos.color = new Color(1, 0.5f, 0.3f,1);
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        Gizmos.DrawIcon(transform.position, "SoundBox");
    }
}
