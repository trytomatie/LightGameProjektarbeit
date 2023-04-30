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



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        aggroTarget = playerTransform;
    }

    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        enemyController.Animation();
        Movement();
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
