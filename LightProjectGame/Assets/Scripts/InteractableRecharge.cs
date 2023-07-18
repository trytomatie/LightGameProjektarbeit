using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InteractableRecharge : Interactable
{

    /// <summary>
    /// Method of Interaction
    /// </summary>
    /// <param name="soruce"></param>
    public override void Interaction(GameObject soruce)
    {
        soruce.GetComponent<StatusManager>().LightEnergy = 1000;
        soruce.GetComponentInChildren<InteractionHandler>().RemoveInteraction(this);
        interactionEvent.Invoke();
        Destroy(gameObject);
    }






}

