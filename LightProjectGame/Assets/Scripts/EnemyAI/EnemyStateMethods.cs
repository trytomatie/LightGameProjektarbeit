using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMethods : MonoBehaviour
{
    [HideInInspector]
    public EnemyStateVarriables esv;
    private Vector3 currentMovePos;

    private void Start()
    {
        esv = GetComponent<EnemyStateVarriables>();
    }
    public void SetSpeed(float speed)
    {
        esv.agent.speed = speed;
    }

    public void MoveToPosition(Vector3 pos)
    {
        currentMovePos = pos;
        esv.agent.SetDestination(pos);
    }

    public void MoveToDirection(Vector3 dir)
    {
        esv.agent.SetDestination(transform.position + dir);
    }

    public void MoveToDirection(Vector3 dir, float multiplier)
    {
        esv.agent.SetDestination(transform.position + (dir * multiplier));
    }

    public void Animation()
    {
        esv.anim.SetFloat(esv.animSpeedHash, esv.agent.velocity.magnitude);
    }

    public void AnimationsParemetersInput()
    {

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 movement = esv.agent.velocity;
        float dot = Vector3.Dot(forward, movement.normalized);
        float dotRight = Vector3.Dot(right, movement.normalized);

        float maxDot = Mathf.Max(Mathf.Abs(dotRight), Mathf.Abs(dot));
        if (maxDot == Mathf.Abs(dot))
        {
            dotRight = 0;
        }
        else
        {
            dot = 0;
        }

        esv.anim.SetFloat(esv.animXInputHash, Mathf.RoundToInt(dotRight) * esv.Speed/1.8f, 0.1f, Time.deltaTime);
        esv.anim.SetFloat(esv.animYInputHash, Mathf.RoundToInt(dot) * esv.Speed /1.8f, 0.1f, Time.deltaTime);
    }

    public void Attack()
    {
        esv.anim.SetBool(esv.animAttackHash, true);
        esv.anim.SetFloat(esv.animSpeedHash, 0);
    }

    public void GetPossibleTargets()
    {
        esv.possibleTargets = GameManager.enemyTargetsInScene;
    }

    public void RotateToPos(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float rotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 3000 * Time.deltaTime);
    }

    public bool HasLoSTarget()
    {
        Vector3 direction = esv.target.Direction(transform.position);
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle <= esv.horizontalFieldOfView / 2f && Mathf.Abs(Vector3.Angle(direction, transform.up)) <= esv.verticalFieldOfView / 2f && esv.target.Distance(transform.position) < esv.aggroRange)
        {
            if (esv.target.HasLoS(esv.EyePosition, esv.aggroRange, esv.layerMask))
            {
                return true;
            }
        }
        return false;
    }

    public TargetInfo CheckLoSPossibleTarget()
    {
        TargetInfo result = null;
        GetPossibleTargets();
        foreach (TargetInfo possibleTarget in esv.possibleTargets)
        {
            if(esv.ignoreLoS && possibleTarget.Distance(transform.position) < esv.aggroRange)
            {
                result = possibleTarget;
            }
            else
            {
                Vector3 direction = possibleTarget.Direction(transform.position);
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle <= esv.horizontalFieldOfView / 2f && Mathf.Abs(Vector3.Angle(direction, transform.up)) <= esv.verticalFieldOfView / 2f && possibleTarget.Distance(transform.position) < esv.aggroRange)
                {
                    if (possibleTarget.HasLoS(esv.EyePosition, esv.aggroRange, esv.layerMask))
                    {
                        result = possibleTarget;
                    }
                }
            }
        }
        return result;
    }


    public int CheckAggroPossition(TargetInfo target)
    {
        for(int i = 0; i < target.aggroList.Count;i++)
        {
            if(gameObject == target.aggroList[i])
            {
                return i;
            }
        }
        return int.MaxValue;
    }

    public bool AttackRoll()
    {
        float attackRoll = Random.value;
        if (attackRoll - GetAggroListAttackRollPenalty(esv.target) > 0f)
        {
            return true;
        }
        return false;
    }

    public bool AttackRoll(float extraPenalty)
    {
        float attackRoll = Random.value * esv.aggroListAttackRollPenalty[Mathf.Clamp(esv.lightsInRange.Count,0,3)];
        attackRoll = attackRoll - GetAggroListAttackRollPenalty(esv.target) - extraPenalty;
        if (attackRoll > 0f)
        {
            return true;
        }
        return false;
    }

    public float GetAggroListPositionModifier(TargetInfo target)
    {
        return esv.AggrolistPositionModifier[Mathf.Clamp(CheckAggroPossition(target),0,esv.AggrolistPositionModifier.Length-1)];
    }

    public float GetAggroListAttackRollPenalty(TargetInfo target)
    {
        return esv.AggroListAttackRollPenalty[Mathf.Clamp(CheckAggroPossition(target), 0, esv.AggrolistPositionModifier.Length - 1)];
    }



    public Vector3 AngleToVector(float angle)
    {
        // Convert the angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the direction vector using a reference vector (e.g., forward)
        Vector3 referenceVector = Vector3.forward;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * referenceVector;

        return direction;
    }

    public float VectorToAngle(Vector3 direction)
    {
        // Calculate the angle using Atan2 function
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Convert the angle to the range of 0 to 360 degrees
        if (angle < 0)
            angle += 360f;

        return angle;
    }

    public Vector3 AverageLightPosition()
    {
        int i = 0;
        Vector3 pos = Vector3.zero;
        foreach(LightController lc in esv.lightsInRange)
        {
            pos += lc.transform.position;
            i++;
        }
        pos /= i;
        return pos;
    }

    #region Gizomos
    private void OnDrawGizmosSelected()
    {
        // Draw Gizmo for the field of view
        DrawFieldOfView();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(currentMovePos, 0.5f);
    }

    private void DrawFieldOfView()
    {
        EnemyStateVarriables esv = GetComponent<EnemyStateVarriables>();
        float halfHorizontalFOV = esv.horizontalFieldOfView / 2f;
        float halfVerticalFOV = esv.verticalFieldOfView / 2f;

        Gizmos.color = Color.yellow;

        Quaternion leftRayRotation = Quaternion.Euler(0f, -halfHorizontalFOV , 0f);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;

        Quaternion rightRayRotation = Quaternion.Euler(0f, halfHorizontalFOV , 0f);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Quaternion upRayRotation = Quaternion.Euler(-halfVerticalFOV , 0f, 0f);
        Vector3 upRayDirection = upRayRotation * transform.forward;

        Quaternion downRayRotation = Quaternion.Euler(halfVerticalFOV , 0f, 0f);
        Vector3 downRayDirection = downRayRotation * transform.forward;

        // Draw rays for horizontal field of view
        Gizmos.DrawRay(esv.EyePosition, leftRayDirection * esv.aggroRange);
        Gizmos.DrawRay(esv.EyePosition, rightRayDirection * esv.aggroRange);

        // Draw rays for vertical field of view
        Gizmos.DrawRay(esv.EyePosition, upRayDirection * esv.aggroRange);
        Gizmos.DrawRay(esv.EyePosition, downRayDirection * esv.aggroRange);
    }
    #endregion
}
