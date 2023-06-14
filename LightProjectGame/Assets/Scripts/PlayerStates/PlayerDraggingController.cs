using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    private float pickUpDelay = 0.3f;
    private float pickUpDelayTimer = 0;
    public SplineComputer splineComputer;
    public float lineSize = 0.5f;
    public float lineEndSize = 0.5f;
    public Transform staffTip;
    public Transform targetIk;
    private RangedCombarController rcc;
    public VisualEffect pickUpVFX;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        lookingForDragables = GetComponent<LookingForDragables>();
        lockOnState = GetComponent<LockOnState>();
        mainCamera = Camera.main;
        rcc = GetComponent<RangedCombarController>();
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
        targetIk.position = targetrb.transform.position;
        targetrb.velocity = direction *force;
    }

    void Rotation()
    {
        float rotation = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 360 * Time.deltaTime);
    }

    void SetDragPointLength()
    {
        
        if(Input.GetKey(KeyCode.Q))
        {
            dragPointOffset = Mathf.Clamp(dragPointOffset + (8 * Time.deltaTime), dragPointLengthMin, dragPointLengthMax);
        }
        if (Input.GetKey(KeyCode.E))
        {
            dragPointOffset = Mathf.Clamp(dragPointOffset - (8 * Time.deltaTime), dragPointLengthMin, dragPointLengthMax);
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            dragPointOffset = Mathf.Clamp(dragPointOffset + Input.mouseScrollDelta.y, dragPointLengthMin, dragPointLengthMax);
        }
        if(Input.GetAxis("DraggableAxis") != 0)
        {
            dragPointOffset = Mathf.Clamp(dragPointOffset + Input.GetAxis("DraggableAxis") * Time.deltaTime * 8, dragPointLengthMin, dragPointLengthMax);
        }
    }

    private void ToggleSpline(bool value)
    {
        splineComputer.gameObject.SetActive(value);
        splineComputer.SetPoint(0, new SplinePoint(new Vector3(0,-100,0)));
        splineComputer.SetPoint(1, new SplinePoint(new Vector3(0, -100, 0)));
        splineComputer.SetPoint(2, new SplinePoint(new Vector3(0, -100, 0)));
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

    private void PlayPickUPVFX()
    {
        pickUpVFX.transform.position = lookingForDragables.rcTarget.transform.position;
        pickUpVFX.transform.LookAt(transform.position);
        pickUpVFX.transform.eulerAngles += new Vector3(90, 0, 0);
        pickUpVFX.Play();
    }


    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 2);
        playerController.anim.SetFloat("movementMode",1);
        playerController.anim.SetBool("aiming", true);
        targetrb = lookingForDragables.rcTarget.GetComponent<Rigidbody>();
        ToggleSpline(true);
        PlayPickUPVFX();
        //SetSplinePoints();
        rcc.HandleHud(true);

        pickUpDelayTimer = Time.time + pickUpDelay;
        
        // Set Input Animations to 0
        playerController.anim.SetFloat("xInput", 0);
        playerController.anim.SetFloat("yInput", 0);
    }
    public override void UpdateState(GameObject source)
    {
        if(pickUpDelayTimer >= Time.time)
        {
            return;
        }
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
        if (!PlayerController.IsAiming() || Input.GetButtonDown("Shoot"))
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 0);
        playerController.anim.SetFloat("movementMode", 0);
        playerController.anim.SetBool("aiming", false);
        ToggleSpline(false);
        rcc.HandleHud(false);
    }
    #endregion
}
