using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDraggingController : State
{
    private PlayerController playerController;
    private LockOnState lockOnState;
    private LookingForDragables lookingForDragables;
    private Camera mainCamera;
    public LayerMask layerMask;
    private Rigidbody targetrb;
    public float force = 20;
    public float dragPointLengthMin = 1;
    public float dragPointLengthMax = 5;
    private float dragPointOffset = 4;
    public SplineComputer splineComputer;
    public float lineSize = 0.5f;
    public float lineEndSize = 0.5f;
    public Transform staffTip;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        lookingForDragables = GetComponent<LookingForDragables>();
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

    private void ToggleSpline(bool value)
    {
        splineComputer.gameObject.SetActive(value);
    }

    private void SetSplinePoints()
    {
        Vector3 middle = (targetrb.transform.position + staffTip.position) / 2;
        splineComputer.SetPoint(0, new SplinePoint(staffTip.position));
        splineComputer.SetPoint(1, new SplinePoint(middle + new Vector3(0,1,0)));
        splineComputer.SetPoint(2, new SplinePoint(targetrb.transform.position));
        for(int i = 0; i<= 2;i++)
        {
            splineComputer.SetPointSize(i, lineSize);
        }
        splineComputer.SetPointSize(2, lineEndSize);

        splineComputer.Rebuild();
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 2);
        playerController.anim.SetFloat("movementMode",1);
        targetrb = lookingForDragables.rcTarget.GetComponent<Rigidbody>();
        ToggleSpline(true);
        SetSplinePoints();
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
        SetSplinePoints();
        SetDragPointLength();
    }

    public override StateName Transition(GameObject source)
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonDown(0))
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 0);
        playerController.anim.SetFloat("movementMode", 0);
        ToggleSpline(false);
    }
    #endregion
}
