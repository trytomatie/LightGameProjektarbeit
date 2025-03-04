using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualShielding : State
{
    private PlayerController playerController;
    private LockOnState lockOnState;
    private Camera mainCamera;
    public Transform ikTarget;


    private StateMachine sm;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mainCamera = Camera.main;
        lockOnState = GetComponent<LockOnState>();
        sm = GetComponent<StateMachine>();
    }




    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.anim.SetFloat("movementMode", 1);
    }

    public override void UpdateState(GameObject source)
    {
        playerController.Animations();
        playerController.Movement();
        playerController.CalculateGravity();
        lockOnState.AnimationsParemetersInput();
        lockOnState.HandleShielding();
        ikTarget.position = transform.position + transform.forward;
    }

    public override StateName Transition(GameObject source)
    {
        if (Input.GetAxisRaw("Manual Block") <= 0)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.anim.SetFloat("movementMode", 0);
        lockOnState.Shielding(false);
    }
    #endregion
}
