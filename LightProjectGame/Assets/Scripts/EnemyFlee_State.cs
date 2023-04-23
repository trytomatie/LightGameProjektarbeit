using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController))]
public class EnemyFlee_State : State
{
    private EnemyController enemyController;
    public float fleeDistance = 3;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Movement()
    {
        enemyController.agent.destination = transform.position + CalculateFleeDirection();
    }

    private Vector3 CalculateFleeDirection()
    {     
        return -(enemyController.lightSource.position - transform.position).normalized;
    }

    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        Movement();
        if(Vector3.Distance(transform.position,enemyController.lightSource.position) > fleeDistance)
        {
            enemyController.lightSource = null;
        }
    }

    public override StateName Transition(GameObject source)
    {
        if(enemyController.lightSource == null)
        {
            return State.StateName.Controlling;
        }
        return base.Transition(source);
    }
}
