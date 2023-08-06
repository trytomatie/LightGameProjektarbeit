using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionBlockSound : MonoBehaviour
{
    public AudioClip sound;
    public float velcoityTreshhold = 1;
    private Rigidbody rb;
    private bool triggerd = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude > velcoityTreshhold && !triggerd)
        {
            FeedbackManager.PlaySFX(sound);
            triggerd = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        triggerd = false;
    }
}
