using System.Collections;
using UnityEngine;


public class Circling_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    private float changeDirectionTimer;
    private float tauntTimer = 5;
    private float strikeWithinRangeTimer = 0;
    private Vector3 direction;

    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        esv.ciclingStartAngle = esm.VectorToAngle(-esv.target.Direction(transform.position));
        esv.anim.SetFloat(esv.animMovementModeHash, 1);
        esv.currentCircelingAngle = esv.ciclingStartAngle;
        esv.agent.stoppingDistance = 0.1f;
        esv.Speed = 2f;
        tauntTimer = Time.time + Random.Range(esv.attackIntervalls.x, esv.attackIntervalls.y);
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        if(Time.time >= changeDirectionTimer)
        {

            direction = Random.onUnitSphere - (transform.forward * 0.1f);
            direction = direction.normalized;
            changeDirectionTimer = Time.time + Random.Range(1f, 2f);
        }

        esm.RotateToPos(esv.target.Position);
        esm.AnimationsParemetersInput();
        if(esv.target.Distance(transform.position) > esv.circleRange)
        {
            esm.MoveToPosition(transform.position + direction + transform.forward);
        }
        else
        {
            esm.MoveToPosition(transform.position + direction);
        }
        if(esv.target.Distance(transform.position) < esv.attackRange)
        {
            changeDirectionTimer = 0;
            esm.MoveToPosition(transform.position + -transform.forward);
        }

        esm.Animation();
    }

    public override void ExitState(GameObject source)
    {
        esv.anim.SetFloat(esv.animMovementModeHash, 0);
        esv.agent.stoppingDistance = 1.13f;
        esv.Speed = 4;
    }

    public override StateName Transition(GameObject source)
    {
        if(tauntTimer <= Time.time)
        {
            if (esm.AttackRoll())
            {
                return StateName.ApproachForAttack;
            }
            else
            {
                return StateName.Taunting;
            }

        }
        if (esv.target.Distance(transform.position) <= esv.attackRange && strikeWithinRangeTimer <= Time.time)
        {
            strikeWithinRangeTimer = Time.time + 2;
            if(esm.AttackRoll(-0.3f))
            {
                return StateName.Attacking;
            }
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
