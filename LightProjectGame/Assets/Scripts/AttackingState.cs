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
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        dashState = GetComponent<DashingState>();
        mainCamera = Camera.main;
    }

    private void HandleAttack()
    {
        Vector3 movement = Vector3.zero;
        if(stateHasChanged)
        {
             HandleRotation();
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
        originalTurnspeed = playerController.turnspeed;
        playerController.turnspeed = 3000;
    }
    public override void UpdateState(GameObject source)
    {
        playerController.CalculateGravity();
        HandleAttack();
        if(Input.GetMouseButton(0))
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
