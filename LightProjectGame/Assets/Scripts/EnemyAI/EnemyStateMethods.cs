using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMethods : MonoBehaviour
{
    [HideInInspector]
    public EnemyStateVarriables esv;

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

    public void Attack()
    {
        esv.anim.SetBool(esv.animAttackHash, true);
        esv.anim.SetFloat(esv.animSpeedHash, 0);
    }

    public void GetPossibleTargets()
    {
        esv.possibleTargets = GameManager.enemyTargetsInScene.ToArray();
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
            Vector3 direction = possibleTarget.Direction(transform.position);
            float angle = Vector3.Angle(direction, transform.forward);
            print(angle);
            if (angle <= esv.horizontalFieldOfView / 2f && Mathf.Abs(Vector3.Angle(direction, transform.up)) <= esv.verticalFieldOfView / 2f && possibleTarget.Distance(transform.position) < esv.aggroRange)
            {
                print("isInFOV");
                if (possibleTarget.HasLoS(esv.EyePosition, esv.aggroRange, esv.layerMask))
                {
                    print("sawHimBoss");
                    result = possibleTarget;
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

    #region Gizomos
    private void OnDrawGizmosSelected()
    {
        // Draw Gizmo for the field of view
        DrawFieldOfView();
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
