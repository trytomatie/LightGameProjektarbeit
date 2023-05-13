using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class DeadState : State
{

    private void Start()
    {

    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
