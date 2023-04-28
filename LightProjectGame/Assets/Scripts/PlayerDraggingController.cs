using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDraggingController : State
{
    private PlayerController playerController;
    private RangedCombarController rcc;
    private Camera mainCamera;

    private Rigidbody targetrb;
    public float force = 20;
    public float dragPointLengthMin = 1;
    public float dragPointLengthMax = 5;
    private float dragPointOffset = 1;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rcc = GetComponent<RangedCombarController>();
        mainCamera = Camera.main;
    }

    void Dragging()
    {
        Vector3 dragPoint = transform.position + new Vector3(0,1.4f,0) + (mainCamera.transform.forward * dragPointOffset);
        Vector3 direction = dragPoint - targetrb.position;
        targetrb.velocity = direction *force;
    }

    void Rotation()
    {
        float rotation = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 360 * Time.deltaTime);
    }

    void SetDragPointLength()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            dragPointOffset = Mathf.Clamp(dragPointOffset + Input.mouseScrollDelta.y, dragPointLengthMin, dragPointLengthMax);
        }
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 2);
        targetrb = rcc.rcTarget.GetComponent<Rigidbody>();
    }
    public override void UpdateState(GameObject source)
    {
        playerController.Movement();
        Rotation();
        playerController.Animations();
        playerController.CalculateGravity();
        playerController.HandleLantern();
        Dragging();
        SetDragPointLength();
    }

    public override StateName Transition(GameObject source)
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonUp(1))
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 0);
    }
    #endregion
}
