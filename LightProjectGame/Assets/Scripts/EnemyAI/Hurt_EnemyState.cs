using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class Hurt_EnemyState : HurtState
{

    private EnemyStateVarriables esv;
    private EnemyStateMethods esm;

    public override void Start()
    {
        base.Start();
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
        anim = esv.anim;

    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        if(esv.target != null)
        {
            esv.target.MoveInAggroList(esm.CheckAggroPossition(esv.target), 2);
        }
        base.EnterState(source);
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        return base.Transition(source);
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
