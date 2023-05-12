using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBox : MonoBehaviour
{
    public UnityEvent triggerEvent;
    public bool singleTrigger = true;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (singleTrigger && triggered)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            triggerEvent.Invoke();
            triggered = true;
        }
    }
}
