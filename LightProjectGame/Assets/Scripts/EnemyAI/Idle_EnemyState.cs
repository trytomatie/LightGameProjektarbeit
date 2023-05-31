using System.Collections;
using UnityEngine;


public class Idle_EnemyState : State
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
        esm.Animation();
    }

    public override void ExitState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        TargetInfo target = esm.CheckLoSPossibleTarget();
        if (target != null)
        {
            print("found target");
            esv.target = target;
            target.aggroList.Add(gameObject);
            return StateName.Alerted;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
