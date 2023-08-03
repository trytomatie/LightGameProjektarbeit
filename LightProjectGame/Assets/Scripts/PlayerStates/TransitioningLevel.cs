using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class TransitioningLevel : State
{
    private PlayerController pc;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {

        pc.myStatus.isInvulnerable = true;
    }
    public override void UpdateState(GameObject source)
    {
        pc.ManualMove(pc.lastMovement,pc.runSpeed);
        pc.CalculateGravityAndApplyForce();
        pc.Animations();
    }

    public override StateName Transition(GameObject source)
    {
        if(!GameManager.instance.isLoadingLevel)
        {
            return State.StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

        pc.myStatus.isInvulnerable = false;
    }
    #endregion
}
