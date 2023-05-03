using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : State
{
    private PlayerController playerController;
    private Camera mainCamera;
    public AnimationCurve attack1;
    public AnimationCurve attack2;
    public AnimationCurve attack3;

    private AnimatorStateInfo currentState;
    private bool stateHasChanged = false;

    private DashingState dashState;
    private float originalTurnspeed;

    public List<EnemyController> enemyList = new List<EnemyController>();

    private bool queueAttack = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        dashState = GetComponent<DashingState>();
        mainCamera = Camera.main;
    }

    private void ScanForEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 8,Vector3.forward);
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.GetComponent<EnemyController>() != null)
            {
                enemyList.Add(hit.collider.GetComponent<EnemyController>());
            }
        }
    }

    private float GetClosestEnemy(out EnemyController result)
    {
        float distance = float.MaxValue;
        EnemyController target = null;
        foreach(EnemyController ec in enemyList)
        {
            float newDistance = Vector3.Distance(transform.position, ec.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                target = ec;

            }
        }
        result = target;
        return distance;
    }

    private Vector3 GetEnemyDirection(Transform enemy)
    {
        return -(transform.position - enemy.position).normalized;
    }

    private void HandleAttack()
    {
        Vector3 movement = Vector3.zero;
        if(stateHasChanged)
        {
            queueAttack = false;
            if (enemyList.Count > 0)
            {
                EnemyController ec;
                float distance = GetClosestEnemy(out ec);

                if(distance < 5)
                {
                    Vector3 enemyDirection = GetEnemyDirection(ec.transform);
                    float rotation = Mathf.Atan2(playerController.lastMovement.x, playerController.lastMovement.z) * Mathf.Rad2Deg;
                    float enemyExpectedRotation = Mathf.Atan2(enemyDirection.x, enemyDirection.z) * Mathf.Rad2Deg;
                    float absoluteDifference = Mathf.Abs(rotation - enemyExpectedRotation);
                    playerController.lastMovement = enemyDirection;
                    playerController.Rotation();
                }
                else
                {
                    HandleRotation();
                }
            }
            else
            {
                HandleRotation();
            }
        }

        CurrentState = playerController.anim.GetCurrentAnimatorStateInfo(2);
        if (playerController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack1"))
        {
            movement = transform.forward * attack1.Evaluate(playerController.anim.GetCurrentAnimatorStateInfo(2).normalizedTime);
            playerController.ManualMove(movement,1);
        }
        if (playerController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack2"))
        {
            movement = transform.forward * attack2.Evaluate(playerController.anim.GetCurrentAnimatorStateInfo(2).normalizedTime);
            playerController.ManualMove(movement, 1);
        }
        if (playerController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack3"))
        {
            movement = transform.forward * attack3.Evaluate(playerController.anim.GetCurrentAnimatorStateInfo(2).normalizedTime);
            playerController.ManualMove(movement, 1);
        }
    }

    private void HandleRotation()
    {
        playerController.Movement();
        playerController.Rotation();
    }
        
    private bool CheckIfInAttackAnimation()
    {

        if (playerController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack1"))
        {
            return true;
        }
        if (playerController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack2"))
        {
            return true;
        }
        if (playerController.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack3"))
        {
            return true;
        }
        return false;
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.anim.SetBool("attack", true);
        playerController.anim.SetFloat("speed", 0);
        originalTurnspeed = playerController.turnspeed;
        playerController.turnspeed = 3000;
        enemyList.Clear();
        ScanForEnemies();
        stateHasChanged = true;
        queueAttack = false;
    }
    public override void UpdateState(GameObject source)
    {
        playerController.CalculateGravity();
        HandleAttack();
        if(Input.GetMouseButton(0) && playerController.anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.35f)
        {
            queueAttack = true;
        }
        if(Input.GetMouseButton(0) || queueAttack)
        {
            playerController.anim.SetBool("attack", true);
        }
        else
        {
            playerController.anim.SetBool("attack", false);
        }
    }

    public override StateName Transition(GameObject source)
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 input = new Vector2(verticalInput, horizontalInput);

        if(!playerController.anim.GetBool("attack") && input != new Vector2(0,0) && playerController.anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.8f)
        {
            return StateName.Controlling;
        }

        if (!playerController.anim.GetBool("attack") && !CheckIfInAttackAnimation())
        {
            return StateName.Controlling;
        }
        if (Input.GetKey(KeyCode.LeftShift) && dashState.dashCooldownTimer < Time.time)
        {
            HandleRotation();
            return StateName.Dashing;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.anim.SetBool("attack", false);
        playerController.turnspeed = originalTurnspeed;
        playerController.movementSpeed = 0;
    }
    #endregion

    public AnimatorStateInfo CurrentState 
    { 
        get => currentState;
        set
        { 
            if(currentState.shortNameHash != value.shortNameHash)
            {
                stateHasChanged = true;
                currentState = value;
            }
            else
            {
                stateHasChanged = false;
                currentState = value;
            }
        }

    }
}
