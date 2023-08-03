using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class CutszeneState : State
{
    private PlayerController pc;
    public bool cutszeneFinished = false;
    
    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        GameManager.instance.canvas.SetActive(false);
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(cutszeneFinished)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        GameManager.instance.canvas.SetActive(true);
    }
    #endregion
}
