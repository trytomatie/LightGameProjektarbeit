using System.Collections;
using UnityEngine;


public class Attack_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    private float attackTime;
    private float speedCache;

    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        esv.anim.SetBool(esv.animAttackHash,true);
        attackTime = Time.time + 1f;
        speedCache = esv.Speed;
        esv.Speed = 0;
        esv.anim.SetFloat(esv.animSpeedHash, 0);
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        if(attackTime <= Time.time)
        {
            esv.anim.SetBool(esv.animAttackHash, false);
        }

    }

    public override void ExitState(GameObject source)
    {
        esv.Speed = speedCache;
    }

    public override StateName Transition(GameObject source)
    {
        if (attackTime <= Time.time && !esv.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack 2"))
        {
            return StateName.AttackRecovery;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
