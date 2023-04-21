using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class InteractionHandler : State
{

    private Animator anim;

    private Interactable reachableInteractable;
    public bool canInteract = false;
    public float interactionDistance = 0.7f;
    public float interactionAngleThreshold = 45;
    public Transform itemAnchor;
    public Transform handIkTarget;
    public Transform lookIkTarget;

    private List<Interactable> potentialInteractables = new List<Interactable>();
    private bool isInteracting = false;
    private Camera mainCamera;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        if(potentialInteractables.Count > 0)
        {
            ReachableInteractable = potentialInteractables.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).First();
            if (ReachableInteractable != null && Helper.DistanceBetween(gameObject, ReachableInteractable.gameObject) > interactionDistance && Helper.AngleBetween(gameObject, ReachableInteractable.gameObject) < interactionAngleThreshold)
            {
                ReachableInteractable.ShowReticleText();
                canInteract = true;
            }
            else
            {
                ReachableInteractable.HideReticleText();
                canInteract = false;
            }
        }
    }

    public void TriggerAnimationEvent(AnimationEvent ae)
    {
        if (ae.stringParameter == "OnGrabStart")
        {
            ReachableInteractable.transform.parent = itemAnchor;
            ReachableInteractable.transform.localPosition = Vector3.zero;
            ReachableInteractable.transform.rotation = new Quaternion(0, 0, 0, 0);
            ReachableInteractable.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (ae.stringParameter == "OnGrabComplete" || ae.stringParameter == "OnInteractionComplete")
        {
            anim.SetTrigger("interactionComplete");
            isInteracting = false;
            canInteract = false;
            ReachableInteractable.Interaction(gameObject);

        }
    }
    /// <summary>
    /// Method that gets called when an object enters the InteractionTrigger
    /// </summary>
    /// <param name="other"></param>
    public void EnterTrigger(Collider other)
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
    public void ExitTrigger(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
            if (!isInteracting)
            {
                potentialInteractables.Remove(other.gameObject.GetComponent<Interactable>());
                interactable.IsReachable = false;
            }
        }
    }

    #region StateMachine Methods
    public override void UpdateState(GameObject source)
    {
        if (ReachableInteractable != null)
        {
            lookIkTarget.transform.position = ReachableInteractable.transform.position;
        }
    }


    public override void EnterState(GameObject source)
    {
        // TODO: Finish fix this stuff!
        return;
        /*
        if (ReachableInteractable.GetComponent<Interactable_Item>() != null)
        {
            reachableInteractable.TriggerAnimation(source);
            handIkTarget.transform.position = ReachableInteractable.transform.position + new Vector3(0, 0.08f, 0);
        }
        else if (ReachableInteractable.GetComponent<Interactable>() != null)
        {
            reachableInteractable.TriggerAnimation(source);
            if (ReachableInteractable.ikTarget != null)
            {
                handIkTarget.transform.position = ReachableInteractable.GetComponent<Interactable>().ikTarget.transform.position;
            }
        }
        else
        {
            return;
        }

        isInteracting = true;

        transform.rotation = Quaternion.LookRotation(new Vector3(ReachableInteractable.transform.position.x, 0, ReachableInteractable.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), Vector3.up);
        */
    }

    public override void ExitState(GameObject source)
    {
        // TODO: FIX THIS ALSO!
        return;
        /*
        if (ReachableInteractable != null && ReachableInteractable.GetComponent<Interactable_Item>() != null)
        {
            potentialInteractables.Remove(ReachableInteractable);
            Destroy(ReachableInteractable.gameObject);
        }
        ReachableInteractable = null;
        */
    }

    public override StateName Transition(GameObject source)
    {
        if (isInteracting == false)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    #endregion


    public Interactable ReachableInteractable 
    { get => reachableInteractable;
        set 
        {
            if(reachableInteractable != value && reachableInteractable != null)
            {
                reachableInteractable.HideReticleText();
            }
            reachableInteractable = value;
        } 
    }
}
