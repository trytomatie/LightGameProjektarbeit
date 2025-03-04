using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class Edgeing : State
{
    private PlayerController playerController;
    private CharacterController cc;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        cc = GetComponent<CharacterController>();
    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        cc.enabled = false;
        transform.position = playerController.edgePosition +cc.center;
        playerController.anim.SetBool("hanging", true);
        cc.enabled = true;
        playerController.transform.rotation = Quaternion.Euler(0, playerController.edgeRotation, 0);

    }
    public override void UpdateState(GameObject source)
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerController.Jump();
            playerController.edgeDetected = false;
        }

    }

    public override StateName Transition(GameObject source)
    {
        if(!playerController.edgeDetected)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.anim.SetBool("hanging", false);
    }
    #endregion


}
