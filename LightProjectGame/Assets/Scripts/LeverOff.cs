using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class LeverOff : State
{
    private LeverOn leverOn;

    public UnityEvent eventOff;

    private void Start()
    {
        leverOn = GetComponent<LeverOn>();

    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        eventOff.Invoke();
        leverOn.anim.SetBool("animate", false);
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(leverOn.isOn)
        {
            return State.StateName.LampOn;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
