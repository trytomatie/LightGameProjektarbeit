using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandlerCollider : MonoBehaviour
{

    public InteractionHandler handler;
    private void OnTriggerEnter(Collider other)
    {
        handler.EnterTrigger(other);
    }

    private void OnTriggerExit(Collider other)
    {
        handler.ExitTrigger(other);
    }
}
