using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDraggingController : State
{
    private PlayerController playerController;
    private LockOnState lockOnState;
    private RangedCombarController rcc;
    private Camera mainCamera;
    public LayerMask layerMask;
    private Rigidbody targetrb;
    public float force = 20;
    public float dragPointLengthMin = 1;
    public float dragPointLengthMax = 5;
    private float dragPointOffset = 1;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rcc = GetComponent<RangedCombarController>();
        lockOnState = GetComponent<LockOnState>();
        mainCamera = Camera.main;
    }

    void Dragging()
    {
        Vector3 dragPoint = transform.position + new Vector3(0,1.4f,0) + (mainCamera.transform.forward * dragPointOffset);
        Vector3 direction = dragPoint - targetrb.position;
        RaycastHit raycast;
        if (Physics.SphereCast(transform.position,0.2f,Vector3.down, out raycast,1f, layerMask))
        {
            print(raycast.collider.gameObject.name);
            if(raycast.collider.gameObject == targetrb.gameObject)
            {
                targetrb.velocity = new Vector3(0, -9, 0);
                return;
            }
        }

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
        playerController.anim.SetFloat("movementMode",1);
        targetrb = rcc.rcTarget.GetComponent<Rigidbody>();
    }
    public override void UpdateState(GameObject source)
    {
        playerController.Movement();
        Rotation();
        playerController.Animations();
        playerController.CalculateGravity();
        playerController.HandleLantern();
        lockOnState.AnimationsParemetersInput();
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
        playerController.anim.SetFloat("movementMode", 0);
    }
    #endregion
}
