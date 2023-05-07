using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class HurtState : State
{
    public Animator anim;
    public bool isHit = false;
    private int frameCount = 0;
    private StateMachine sm;

    private void Start()
    {
        sm = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if(isHit)
        {
            frameCount++;
            if(frameCount > 1)
            {
                frameCount = 0;
                isHit = false;
            }
        }
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        print("i triggerd");
        anim.SetTrigger("hit");
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(!isHit)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {
        if(isHit && (sm.currentState.stateName != State.StateName.Attacking && sm.currentState.stateName != State.StateName.Dashing) )
        {
            return StateName.Hurt;
        }
        return base.AnyTransition(source);
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
