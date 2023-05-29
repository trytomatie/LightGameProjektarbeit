using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class Fleeing_EnemyState : State
{

    private EnemyStateVarriables esv;
    private EnemyStateMethods esm;

    public void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {

    }
    public override void UpdateState(GameObject source)
    {
        Vector3 lightDirection = (transform.position - esm.AverageLightPosition()).normalized * 3;
        esm.MoveToPosition(transform.position + lightDirection);
        esm.Animation(); 
    }

    public override StateName Transition(GameObject source)
    {
        if (esv.lightsInRange.Count <= 0)
        {
            if(esv.target.sm != null)
            {
                return StateName.Circling;
            }
            return StateName.Idle;
        }
        return base.Transition(source);
    }

    public override StateName AnyTransition(GameObject source)
    {
        if(esv.lightsInRange.Count > 0 && esv.stateMachine.currentState.stateName != StateName.Hurt)
        {
            return StateName.Flee;
        }
        return base.AnyTransition(source);
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
