using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionName;
    public Vector3 labelOffset;
    public Transform ikTarget;
    public GameObject reticle;
    private GameObject reticleInstance;
    private Animator reticleAnimator;
    private Transform reticleHolder;
    private bool isReachable;

    private void Awake()
    {
        reticleHolder = GameObject.Find("ReticleHolder").transform;
    }

    /// <summary>
    /// Method of Interaction
    /// </summary>
    /// <param name="soruce"></param>
    public virtual void Interaction(GameObject soruce)
    {

    }

    /// <summary>
    /// Method that triggers the Animation of Interaction
    /// </summary>
    /// <param name="source"></param>
    public virtual void TriggerAnimation(GameObject source)
    {

    }


    public bool IsReachable
    {
        get => isReachable;
        set
        {
            if (value != isReachable)
            {
                if (value == true)
                {
                    SpawnReticle();
                }
                else
                {
                    DespawnReticle();
                }

            }
            isReachable = value;
        }
    }

    /// <summary>
    /// Spawn the UI reticle
    /// </summary>
    private void SpawnReticle()
    {
        if (reticleInstance == null)
        {
            reticleInstance = Instantiate(reticle, transform.position, Quaternion.identity, reticleHolder);
//            reticleInstance.GetComponent<ReticleUI>().target = transform;
            reticleAnimator = reticleInstance.GetComponent<Animator>();
            reticleInstance.GetComponentInChildren<TextMeshProUGUI>().text = interactionName;
        }
    }

    /// <summary>
    /// Despawn the UI reticle
    /// </summary>
    private void DespawnReticle()
    {
        reticleAnimator.SetTrigger("Despawn");
        Destroy(reticleInstance, 0.5f);
        reticleInstance = null;
    }

    /// <summary>
    /// Show text of reticle
    /// </summary>
    public void ShowReticleText()
    {
        if (reticleAnimator != null) reticleAnimator.SetBool("ShowText", true);

    }

    /// <summary>
    /// Despawn Text of Reticle
    /// </summary>
    public void HideReticleText()
    {
        if (reticleAnimator != null) reticleAnimator.SetBool("ShowText", false);
    }

}

