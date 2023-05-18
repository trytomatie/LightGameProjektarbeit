using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController))]
public class EnemyFlee_State : State
{
    private EnemyController enemyController;
    public float fleeDistance = 3;
    public float fleeDirectionMutliplier = 3;
    public float maxFleeTime = 4;
    private float fleeTime = 0;

    private Vector3 averageLightsourcePos;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Movement()
    {
        enemyController.agent.destination = transform.position + CalculateFleeDirection() * fleeDirectionMutliplier;
    }

    private Vector3 CalculateFleeDirection()
    {
        
        Vector3 averageLightsourcePos = Vector3.zero;
        foreach(Transform pos in enemyController.lightSources)
        {
            averageLightsourcePos += pos.position;
        }
        if(enemyController.lightSources.Count <= 0)
        {
            return -(this.averageLightsourcePos - transform.position).normalized;
        }
        averageLightsourcePos /= enemyController.lightSources.Count;
        this.averageLightsourcePos = averageLightsourcePos;
        return -(averageLightsourcePos - transform.position).normalized;
    }

    public override void EnterState(GameObject source)
    {
        fleeTime = 0;
    }

    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        fleeTime += Time.deltaTime;
        enemyController.Animation();
        Movement();
    }

    public override StateName Transition(GameObject source)
    {
        if(enemyController.lightSources.Count <= 0 && Vector3.Distance(transform.position,averageLightsourcePos) > fleeDistance)
        {
            return State.StateName.Controlling;
        }
        if(fleeTime > maxFleeTime)
        {
            return StateName.Controlling;
        }
        return base.Transition(source);
    }

    public override StateName AnyTransition(GameObject source)
    {
        if (enemyController.lightSources.Count > 0 && stateName!=State.StateName.Attacking)
        {
            return State.StateName.Flee;
        }
        return base.AnyTransition(source);
    }
}
