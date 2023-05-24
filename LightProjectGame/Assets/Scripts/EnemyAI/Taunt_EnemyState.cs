using System.Collections;
using UnityEngine;


public class Taunt_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    private float tauntTime = 0.5f;


    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        esv.anim.SetTrigger(esv.animTauntHash);
        tauntTime = Time.time + 0.5f;
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
        if(tauntTime <= Time.time && !esv.anim.GetCurrentAnimatorStateInfo(2).IsName("Taunt"))
        {
            return esv.previousState.stateName;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
