using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class HurtStaggerState : State
{
    private float staggerTime = 0.5f;
    private float staggerTimer = 0;
    private StatusManager sm;


    private void Start()
    {
        sm = GetComponent<StatusManager>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        if(staggerTimer + staggerTime <= Time.time)
        {
            staggerTimer = Time.time + staggerTime;
        }
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(staggerTimer <= Time.time)
        {
            if(sm.faction == StatusManager.Faction.Enemy)
            {
                return StateName.ApproachForAttack;
            }
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
