using System.Collections;
using UnityEngine;


public class Approach_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    private Vector3 direction;
    private float timer = 0;

    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        direction = Random.onUnitSphere - (transform.forward * 0.1f);
        direction = direction.normalized;
        esv.Speed = 4.5f;
        timer = Time.time + 5;
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        esm.RotateToPos(esv.target.Position);
        esm.MoveToPosition(transform.position + transform.forward + direction);
        esm.Animation();
    }

    public override void ExitState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(esv.target.Distance(transform.position) <= esv.circleRange)
        {
            if(esm.AttackRoll())
            {
                return StateName.ApproachForAttack;
            }
            else
            {
                return StateName.Circling;
            }

        }
        if(timer >= Time.time)
        {
            return StateName.Circling;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
