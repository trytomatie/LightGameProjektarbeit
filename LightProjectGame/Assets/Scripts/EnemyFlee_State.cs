using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController))]
public class EnemyFlee_State : State
{
    private EnemyController enemyController;
    public float fleeDistance = 3;

    private Vector3 averageLightsourcePos;
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
        Vector3 averageLightsourcePos = Vector3.zero;
        foreach(Transform pos in enemyController.lightSources)
        {
            averageLightsourcePos += pos.position;
        }
        averageLightsourcePos /= enemyController.lightSources.Count;
        return -(averageLightsourcePos - transform.position).normalized;
    }

    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        Movement();
    }

    public override StateName Transition(GameObject source)
    {
        if(enemyController.lightSources.Count <= 0)
        {
            return State.StateName.Controlling;
        }
        return base.Transition(source);
    }
}
