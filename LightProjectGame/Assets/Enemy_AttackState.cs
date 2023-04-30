using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : State
{
    public AnimationCurve attack1;

    private float originalTurnspeed;
    private EnemyController enemyController;
    public float attackCooldown = 3.5f;
    public float attackCooldownTimer = 0;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private bool CheckIfInAttackAnimation()
    {

        if (enemyController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack1"))
        {
            return true;
        }
        if (enemyController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack2"))
        {
            return true;
        }
        if (enemyController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack3"))
        {
            return true;
        }
        return false;
    }


    private void HandleAttack()
    {
        Vector3 movement = Vector3.zero;
        if (enemyController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack1"))
        {
            movement = transform.forward * attack1.Evaluate(enemyController.anim.GetCurrentAnimatorStateInfo(2).normalizedTime);
            enemyController.anim.SetBool("attack", false);
        }
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        enemyController.anim.SetBool("attack", true);
        enemyController.anim.SetFloat("speed", 0);
    }
    public override void UpdateState(GameObject source)
    {
        HandleAttack();
    }

    public override StateName Transition(GameObject source)
    {
        if (!CheckIfInAttackAnimation())
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        enemyController.anim.SetBool("attack", false);
        attackCooldownTimer = Time.time+ attackCooldown;
        // enemyController.turnspeed = originalTurnspeed;
        // enemyController.movementSpeed = 0;
    }
    #endregion
}
