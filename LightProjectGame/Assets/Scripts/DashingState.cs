using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : State
{
    private PlayerController playerController;
    private Camera mainCamera;

    [Header("Dash")]
    private bool isDashing = false;
    public float dashTime = 0.5f;
    public float dashSpeed = 30;
    public float dashCooldown = 0.5f;
    public AnimationCurve dashCurve;
    private float dashTimer = 0;
    public float dashCooldownTimer = 0;
    private Vector3 dashDirection;



    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }


    private void HandleDash()
    {
            dashDirection = playerController.lastMovement;
            isDashing = true;;
            playerController.anim.SetBool("dash", true);
            dashTimer = Time.time;
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        HandleDash();
    }
    public override void UpdateState(GameObject source)
    {
        playerController.CalculateGravity();
        if (!playerController.Dash(dashCurve, dashDirection, dashTimer, dashSpeed, dashTime))
        {
            isDashing = false;
        }
    }

    public override StateName Transition(GameObject source)
    {
        if (!isDashing)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        isDashing = false;
        playerController.anim.SetBool("dash", false);
        dashCooldownTimer = Time.time + dashCooldown;
    }
    #endregion
}
