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

