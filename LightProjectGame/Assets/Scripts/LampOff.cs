using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class LampOff : State
{
    private LampOn lampOn;


    private void Start()
    {
        lampOn = GetComponent<LampOn>();
    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        lampOn.lc.TurnOff();
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(lampOn.isOn)
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
