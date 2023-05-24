using System.Collections;
using UnityEngine;


public class AttackRecovery_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    private float timer;

    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        timer = Time.time + 1f;
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {

    }

    public override void ExitState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(timer <= Time.time)
        {
            if(esm.AttackRoll(0.2f))
            {
                return StateName.ApproachForAttack;
            }
            else
            {
                return StateName.Circling;
            }

        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
