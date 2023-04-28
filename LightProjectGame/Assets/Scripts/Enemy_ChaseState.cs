using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController))]
public class Enemy_ChaseState : State
{
    private EnemyController enemyController;
    public float aggroRadius = 7;
    public Transform aggroTarget;

    private StateMachine sm;

    private Transform playerTransform;

    // Start is called before the first frame updat
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        playerTransform = GameObject.Find("Player").transform;
        sm = GetComponent<StateMachine>();
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
