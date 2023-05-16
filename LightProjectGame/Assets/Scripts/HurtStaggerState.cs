using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class HurtStaggerState : State
{
    private float staggerTime = 0.5f;
    private float staggerTimer = 0;


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        staggerTimer = Time.time + staggerTime;
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(staggerTimer <= Time.time)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {
        return base.AnyTransition(source);
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
