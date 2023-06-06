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

    private void OnDrawGizmos()
    {
        Collider col = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0, 1f, 0f, 0.5f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        Gizmos.color = new Color(0, 1f, 0f, 1f);
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
