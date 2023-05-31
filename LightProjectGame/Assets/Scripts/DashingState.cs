using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : State
{
    private PlayerController playerController;
    private Camera mainCamera;
    private CharacterController cc;

    [Header("Dash")]
    private bool isDashing = false;
    public float dashTime = 0.5f;
    public float dashSpeed = 30;
    public float dashCooldown = 0.5f;
    public AnimationCurve dashCurve;
    private float dashTimer = 0;
    public float dashCooldownTimer = 0;
    private Vector3 dashDirection;

    public float duckHeight = 0.5f;
    public float duckOffset = -0.18f;

    private float originalHight;
    private float originalOffset;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        cc = GetComponent<CharacterController>();
        originalHight = cc.height;
        originalOffset = cc.center.y;
    }


    private void HandleDash()
    {
        dashDirection = playerController.lastMovement;
        float ts = playerController.turnspeed;
        playerController.turnspeed = 3000;
        playerController.Rotation();
        playerController.turnspeed = ts;
        isDashing = true;
        playerController.anim.SetBool("dash", true);
        dashTimer = Time.time;
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        cc.height = duckHeight;
        cc.center = new Vector3(0, duckOffset, 0);
        playerController.myStatus.isInvulnerable = true;
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
        playerController.myStatus.isInvulnerable = false;
        dashCooldownTimer = Time.time + dashCooldown;
        cc.height = originalHight;
        cc.center = new Vector3(0, originalOffset, 0);
    }
    #endregion
}
