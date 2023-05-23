using System.Collections;
using UnityEngine;


public class Alerted_EnemyState : State
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
        esv.anim.SetTrigger(esv.animAlertedHash);
        esv.agent.updateRotation = false;
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        esm.RotateToPos(esv.target.Position);
    }

    public override void ExitState(GameObject source)
    {
        esv.agent.updateRotation = true;
    }

    public override StateName Transition(GameObject source)
    {
        if(!esv.anim.GetCurrentAnimatorStateInfo(2).IsName("Alerted"))
        {
            return StateName.Approaching;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
