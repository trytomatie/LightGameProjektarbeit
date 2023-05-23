using System.Collections;
using UnityEngine;


public class Aproaching_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;


    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {

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

    }

    public override StateName Transition(GameObject source)
    {
        if(esv.target.Distance(transform.position) <= esv.circleRange)
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
