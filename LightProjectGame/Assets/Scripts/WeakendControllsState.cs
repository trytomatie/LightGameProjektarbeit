using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class WeakendControllsState : State
{
    private PlayerController pc;
    public GameObject lampModel;
    public LightController lc;
    private StatusManager sm;

    private DashingState dashState;

    public bool switchToHeightendState = false;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        sm = GetComponent<StatusManager>();
        dashState = GetComponent<DashingState>();
    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        lc.TurnOff();
        lampModel.SetActive(false);
        sm.shieldHp = 0;
        sm.shieldMaxHp = 0;
    }
    public override void UpdateState(GameObject source)
    {
        if (pc.isTransitioning)
        {
            pc.isTransitioning = false;
            return;
        }
        pc.HandleJump();
        pc.CalculateGravity();
        pc.Movement();
        pc.Rotation();
        pc.Animations();
    }

    public override StateName Transition(GameObject source)
    {
        if (Input.GetKey(KeyCode.LeftShift) && dashState.dashCooldownTimer < Time.time)
        {
            return StateName.Dashing;
        }
        if (Input.GetMouseButton(1))
        {
            pc.camAnim.SetInteger("cam", 1);
            return StateName.Controlling;
        }
        else
        {
            pc.camAnim.SetInteger("cam", 0);
        }
        if (Input.GetMouseButton(0))
        {
            return StateName.Attacking;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            return pc.HandleTargeting();
        }
        if (pc.edgeDetected)
        {
            pc.anim.SetBool("jumping", false);
            return StateName.Edgeing;
        }
        if(switchToHeightendState)
        {
            lc.TurnOn();
            switchToHeightendState = false;
            lampModel.SetActive(true);
            sm.shieldHp = 30;
            sm.shieldMaxHp = 30;
            stateName = StateName.Invalid;
            pc.stateName = StateName.Controlling;
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion


}
