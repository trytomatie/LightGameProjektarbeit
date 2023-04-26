using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class LampOn : State
{
    public LightController lc;
    private StateMachine sm;
    public bool isOn = true;


    private void Start()
    {
        sm = GetComponent<StateMachine>();
    }

    public void ChangeState()
    {
        isOn = !isOn;
        sm.ManualUpdate();
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        lc.TurnOn();
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(!isOn)
        {
            return State.StateName.LampOff;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
