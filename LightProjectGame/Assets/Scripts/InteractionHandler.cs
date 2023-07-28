using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class InteractionHandler : MonoBehaviour
{
    private Interactable reachableInteractable;
    private bool canInteract = false;
    public float interactionDistance = 0.7f;
    public float interactionAngleThreshold = 45;
    public GameObject interactUI;

    private List<Interactable> potentialInteractables = new List<Interactable>();
    private List<Interactable> removalList = new List<Interactable>();
    private bool isInteracting = false;

    internal void RemoveInteraction(Interactable interactable)
    {
        removalList.Add(interactable);
        CanInteract = false;
    }

    private Camera mainCamera;



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckForInteraction();
    }

    /// <summary>
    /// Checks if an Interaction is available
    /// </summary>
    private void CheckForInteraction()
    {
        if(removalList.Count > 0)
        {
            foreach(Interactable interactable in removalList)
            {
                potentialInteractables.Remove(interactable);
            }
        }
        if(potentialInteractables.Count > 0)
        {
            ReachableInteractable = potentialInteractables.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).First();
            if (ReachableInteractable != null && Helper.DistanceBetween(gameObject, ReachableInteractable.gameObject) > interactionDistance && Helper.AngleBetween(gameObject, ReachableInteractable.gameObject) < 180)
            {
                CanInteract = true;
            }
            else
            {
                CanInteract = false;
            }
        }
    }

    /// <summary>
    /// Method that gets called when an object enters the InteractionTrigger
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
            if (!isInteracting && !potentialInteractables.Contains(interactable))
            {
                potentialInteractables.Add(interactable);
                interactable.IsReachable = true;
            }
        }

    }
    /// <summary>
    /// Method that gets called when an object leaves the InteractionTrigger
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
            potentialInteractables.Remove(other.gameObject.GetComponent<Interactable>());
            interactable.IsReachable = false;
            if(potentialInteractables.Count <= 0)
            {
                CanInteract = false;
            }
        }
    }


    public Interactable ReachableInteractable 
    { get => reachableInteractable;
        set 
        {
            reachableInteractable = value;
            
        } 
    }

    public bool CanInteract { get => canInteract; set
        {
            if (value != canInteract)
            {
                if(value)
                {
                    GameManager.SpawnInteractText(ReachableInteractable.transform.position + new Vector3(0, 2, 0));
                }
                else
                {
                    GameManager.DespawnInteractText();
                }
            }
            canInteract = value;
            // interactUI.SetActive(value);

        } 
    }
}
