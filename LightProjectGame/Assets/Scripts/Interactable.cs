using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string interactionName;
    private bool isReachable;
    public UnityEvent interactionEvent;
    public AudioClip sound;
    public bool singleUse = false;
    public bool used = false;

    private void Awake()
    {

    }

    /// <summary>
    /// Method of Interaction
    /// </summary>
    /// <param name="soruce"></param>
    public virtual void Interaction(GameObject soruce)
    {

        interactionEvent.Invoke();
        if(sound != null)
        {
            FeedbackManager.PlaySFX(sound);
        }
        if(singleUse)
        {
            used = true;
        }


    }

    public bool IsReachable
    {
        get => isReachable;
        set
        {
            isReachable = value;
        }
    }




}

