using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : State
{
    public Transform target;

    public Transform lightSource;

    public Animator anim;

    [HideInInspector]
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        Animation();
    }

    private void Animation()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
    }

    private void Movement()
    {
        agent.destination = target.position;
        print(agent.pathStatus);
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
        if(lightSource != null)
        {
            return State.StateName.Flee;
        }
        return base.Transition(source);
    }
}
