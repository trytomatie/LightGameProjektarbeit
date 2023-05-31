using System.Collections;
using UnityEngine;


public class ApproachAttackRange_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    private float chaseTime;


    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        esv.Speed = 4.5f;
        chaseTime = Time.time + Random.Range(2, 8f);
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        esm.RotateToPos(esv.target.Position);
        esm.MoveToPosition(esv.target.Position);
        esm.Animation();
    }

    public override void ExitState(GameObject source)
    {
        esv.Speed = 4;
    }

    public override StateName Transition(GameObject source)
    {
        if(esv.target == null)
        {
            return StateName.Idle;
        }
        if(esv.target.Distance(transform.position) <= esv.attackRange)
        {
            return StateName.Attacking;
        }
        if(Time.time > chaseTime)
        {
            return StateName.Circling;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
