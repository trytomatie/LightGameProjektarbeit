using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class HurtState : State
{
    public Animator anim;
    public bool isHit = false;
    internal int frameCount = 0;
    internal StateMachine sm;
    internal StatusManager myStatus;
    public UnityEvent shieldingEvent;
    private PlayerController pc;

    public virtual void Start()
    {
        sm = GetComponent<StateMachine>();
        myStatus = GetComponent<StatusManager>();
        pc = GetComponent<PlayerController>();
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
        if (myStatus.shielding)
        {
            shieldingEvent.Invoke();
        }
        if(myStatus.Hp > 0) 
        {
            anim.SetTrigger("hit");
        }

    }
    public override void UpdateState(GameObject source)
    {
        pc.CalculateGravityAndApplyForce();
    }

    public override StateName Transition(GameObject source)
    {
        if(!isHit)
        {
            return StateName.Staggered;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        if(isHit)
        {

            if (myStatus.faction == StatusManager.Faction.Enemy && (sm.currentState.stateName != State.StateName.Attacking && sm.currentState.stateName != StateName.Dead))
            {
                print("I AM HIT!!!!!!");
                return StateName.Hurt;
            }
            if (myStatus.faction == StatusManager.Faction.Player && (sm.currentState.stateName != State.StateName.Shielding))
            {
                return StateName.Hurt;
            }
        }
        return base.AnyTransition(source);
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
