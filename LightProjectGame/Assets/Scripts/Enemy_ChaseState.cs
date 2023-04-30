using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController))]
public class Enemy_ChaseState : State
{
    private EnemyController enemyController;
    public float aggroRadius = 7;
    public float attackRadius = 2;
    public Transform aggroTarget;

    private StateMachine sm;

    private Transform playerTransform;
    private Enemy_AttackState attackState;

    // Start is called before the first frame updat
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        playerTransform = GameObject.Find("Player").transform;
        sm = GetComponent<StateMachine>();
        attackState = GetComponent<Enemy_AttackState>();
    }

    private void Movement()
    {
        enemyController.agent.destination = aggroTarget.position;
    }

    public void Rotation()
    {
        Vector3 dir = (aggroTarget.position - transform.position).normalized;
        float rotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 3000 * Time.deltaTime);
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        aggroTarget = playerTransform;
        enemyController.agent.updateRotation = false;
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        enemyController.Animation();
        Movement();
        Rotation();
    }

    public override void ExitState(GameObject source)
    {
        enemyController.agent.updateRotation = true;
    }

    public override StateName Transition(GameObject source)
    {
        if(Vector3.Distance(playerTransform.position, transform.position) > aggroRadius)
        {
            return StateName.Controlling;
        }
        if(Vector3.Distance(playerTransform.position, transform.position) < attackRadius && attackState.attackCooldownTimer < Time.time)
        {
            return StateName.Attacking;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {
        if(Vector3.Distance(playerTransform.position,transform.position) < aggroRadius && sm.currentState == enemyController)
        {
            return StateName.Chasing;
        }
        return base.AnyTransition(source);
    }
    #endregion
}
