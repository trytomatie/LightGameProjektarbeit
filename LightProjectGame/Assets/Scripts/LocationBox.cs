using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LocationBox : MonoBehaviour
{
    public bool singleTrigger = true;
    private bool triggered = false;
    public string locationText;

    private void OnTriggerEnter(Collider other)
    {
        if (singleTrigger && triggered)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            GameManager.instance.locationText.text = locationText;
            GameManager.instance.locationTextAnimator.SetTrigger("Animate");
            triggered = true;
        }
    }

    private void OnDrawGizmos()
    {
        Collider col = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0, 1f, 0f, 0.2f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        Gizmos.color = new Color(0, 1f, 0f, 1f);
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        Gizmos.DrawIcon(transform.position, "LocationTriggerBox");
    }
}
