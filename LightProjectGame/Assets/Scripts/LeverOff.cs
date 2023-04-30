using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class LeverOff : State
{
    private LeverOn leverOn;

    public UnityEvent eventOff;
    public Material offMaterial;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        leverOn = GetComponent<LeverOn>();
        meshRenderer = GetComponent<MeshRenderer>();
    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        eventOff.Invoke();
        meshRenderer.material = offMaterial;
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
