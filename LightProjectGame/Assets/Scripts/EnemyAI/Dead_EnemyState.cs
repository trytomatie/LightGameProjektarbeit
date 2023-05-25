using System.Collections;
using UnityEngine;


public class Dead_EnemyState : State
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
        esv.target.aggroList.Remove(gameObject);
        esv.anim.SetTrigger("death");
        Destroy(gameObject, 5);
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

        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
