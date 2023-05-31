using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class DeadState : State
{
    private PlayerController pc;
    private int layerMaskCache;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        pc.myStatus.isInvulnerable = true;
        Cursor.lockState = CursorLockMode.None;
        pc.anim.SetTrigger("death");
        GameManager.instance.deathMessageUI.SetActive(true);
        layerMaskCache = gameObject.layer;
        gameObject.layer = 0;
    }
    public override void UpdateState(GameObject source)
    {
        pc.CalculateGravityAndApplyForce();
    }

    public override StateName Transition(GameObject source)
    {
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        GameManager.instance.deathMessageUI.SetActive(false);
        GameManager.instance.deathMessageUI.GetComponent<Animator>().SetBool("TriggerDeath", false);
        pc.myStatus.isInvulnerable = false;
        gameObject.layer = layerMaskCache;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
