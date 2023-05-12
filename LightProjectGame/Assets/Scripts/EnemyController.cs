using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : State
{
    public Transform target;

    public List<Transform> lightSources = new List<Transform>();

    public Animator anim;

    [HideInInspector]
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public void Animation()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
    }

    private void Movement()
    {
        agent.destination = transform.position;
    }

    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        Animation();
        Movement();
    }

    public override StateName Transition(GameObject source)
    {   
        return base.Transition(source);
    }
}
