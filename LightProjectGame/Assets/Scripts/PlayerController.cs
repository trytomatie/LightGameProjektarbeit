using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

/// <summary>
/// By Christian Scherzer
/// </summary>
public class PlayerController : State
{
    public bool startWeakend = true;

    [Header("Speed stats")]
    public float walkSpeed = 2;
    public float  runSpeed = 6;
    public float backwardsSpeed = 6;
    public float acceleration = 7;
    public float turnspeed = 15;
    public const float sneakSpeed = 2f;

    [Header("Physics")]
    public float gravity = -9.81f;
    public bool grounded = false;

    [Header("GroundCheck")]
    public float castDistance = 0.09f;
    public float castScaleFactor = 1;
    public LayerMask layerMask;


    [Header("Sneaking")]
    public bool isSneaking = false;
    public int chaseIndex=0;
    public float noise = 0;

    [Header("Light")]
    public LightController lightController;

    private bool isJumping = false;
    
    [Header("Jump")]
    public float jumpStrength = 5;
    private Vector3 lastHitPoint;
    private Vector3 slideMovement;
    private float timeInTheAir = 0;

    [Header("EdgeDetection")]
    public float edgeDetectionY;
    public float edgeDetectionZ;
    public float edgeDetectionDistance = 0.4f;
    public float edgeSphereCastRadius = 0.5f;
    public bool edgeDetected;
    public Vector3 edgePosition;



    public float movementSpeed = 0;
    private float ySpeed;
    private Vector3 movement;
    [HideInInspector]
    public Vector3 lastMovement;

    [Header("Attack")]
    public VisualEffect slashVFX;

    internal bool isTransitioning = false;

    private CharacterController cc;
    public Animator anim;
    [Header("Camaera")]
    public Animator camAnim;
    public Camera mainCamera;
    public CinemachineRecomposer cm;

    private Volume chaseVolume;
    public AnimationCurve attackCurve;
    public Material normalMaterial;


    private DashingState dashState;
    private LockOnState lockOnState;
    private StateMachine sm;
    private WeakendControllsState weakendState;
    private RaycastHit slopeHit;


    // Start is called before the first frame update
    void Start()
    {
        dashState = GetComponent<DashingState>();
        cc = GetComponent<CharacterController>();
        lockOnState = GetComponent<LockOnState>();
        sm = GetComponent<StateMachine>();
        weakendState = GetComponent<WeakendControllsState>();
        // mainCamera = Camera.main;
        if (startWeakend)
        {
            weakendState.stateName = StateName.Controlling;
            stateName = StateName.Invalid;
        }


    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
    }




    private void LateUpdate()
    {
        if(!(ySpeed - cc.velocity.y > -10))
        {
            //slideMovement = (transform.position - lastHitPoint).normalized * 0.1f;
        }
        else
        {
            //slideMovement = Vector3.zero;
        }
    }

    public void CheckForEdge()
    {
        float scaleOverTime = Mathf.Lerp(0.2f, 1, timeInTheAir / 2);
        float edd = edgeDetectionDistance * scaleOverTime;
        float edy = edgeDetectionY * scaleOverTime;
        RaycastHit rc;
        if(Physics.SphereCast(transform.position + transform.forward * edgeDetectionZ + new Vector3(0, edy, 0), edgeSphereCastRadius, Vector3.down, out rc, edd, layerMask))
        {
            if(rc.normal.y > 0.7f && rc.normal.y < 1.4f)
            {
                RaycastHit rc2;
                for (int i = 1; i < 20; i++)
                {
                    if (Physics.SphereCast(rc.point - transform.forward - new Vector3(0, 0.1f * i, 0), 0.01f, transform.forward, out rc2, edgeDetectionDistance, layerMask))
                    {
                        Debug.DrawLine(rc.point - transform.forward - new Vector3(0, 0.1f * i, 0), rc.point - transform.forward - new Vector3(0, 0.1f * i, 0) + transform.forward, Color.blue, 10);
                        if (rc.normal != rc2.normal)
                        {
                            float dotProduct = Vector3.Dot(rc.normal.normalized, rc2.normal.normalized); // dot product of the normalized vectors
                            float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
                            print(string.Format("Angle:{0} Top:{1} Side:{2}", angle, rc.normal, rc2.normal));
                            if(angle > 85 && angle < 100)
                            {
                                edgeDetected = true;
                                edgePosition = new Vector3(rc2.point.x,rc.point.y,rc2.point.z);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handle Animations
    /// </summary>
    public void Animations()
    {
        anim.SetFloat("speed", movementSpeed);
        anim.SetFloat("ySpeed", ySpeed / 12);
        anim.SetBool("jumping", !grounded);
        transform.position += anim.deltaPosition;
    }

    /// <summary>
    /// Handle Rotation
    /// </summary>
    public void Rotation()
    {
        if (cc.velocity.magnitude > 0)
        {
            float rotation = Mathf.Atan2(lastMovement.x, lastMovement.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), turnspeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Handle Movement
    /// </summary>
    public void Movement()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float targetSpeed = runSpeed;

        if (Input.GetKey(KeyCode.LeftControl) || lockOnState.target != null || sm.currentState.stateName == StateName.Aiming || sm.currentState.stateName == StateName.Dragging)
        {
            targetSpeed = walkSpeed;
        }
        if(isSneaking)
        {
            targetSpeed = sneakSpeed;
        }


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
        {
            timeInTheAir += Time.deltaTime;
            movement = new Vector3(horizontalInput * 1, 0, verticalInput * 1).normalized;
            CheckForEdge();
        }
        else
        {
            timeInTheAir = 0;
            movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        }






        Vector3 cameraDependingMovement = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * movement;
        movementSpeed = Mathf.MoveTowards(movementSpeed, movement.magnitude * targetSpeed, acceleration * Time.deltaTime);

        if (verticalInput != 0 || horizontalInput != 0)
        {
            lastMovement = cameraDependingMovement;
        }
        if(OnSlope())
        {
            lastMovement = GetSlopeMoveDirection(lastMovement);
        }

        cc.Move(lastMovement * movementSpeed * Time.deltaTime 
            + new Vector3(0, ySpeed, 0) * Time.deltaTime
            + slideMovement + anim.deltaPosition);
    }

    public void ManualMove(Vector3 movement,float speed)
    {

        cc.Move(movement * speed * Time.deltaTime
            + new Vector3(0, ySpeed, 0) * Time.deltaTime
            + slideMovement);
    }

    public bool Dash(AnimationCurve dashCurve, Vector3 dashDirection,float dashTimer,float dashSpeed,float dashTotalTime)
    {
        lastMovement = dashDirection;
        float time = Mathf.Clamp01((Time.time - dashTimer) / dashTotalTime);
        movementSpeed = dashCurve.Evaluate(time) * dashSpeed;

        cc.Move(lastMovement * movementSpeed * Time.deltaTime
    + new Vector3(0, ySpeed, 0) * Time.deltaTime
    + slideMovement + anim.deltaPosition);
        if (time >= 1)
        {
            return false;
        }
        
        return true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, cc.height *0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < cc.slopeLimit && angle != 0;
        }
        else
        {
            return false;
        }

    }

    private Vector3 GetSlopeMoveDirection(Vector3 mov)
    {
        return Vector3.ProjectOnPlane(mov, slopeHit.normal).normalized;
    }   

    /// <summary>
    /// Handle Jump Input
    /// </summary>
    public void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping && (grounded || (ySpeed > (-0.5f * 12))))
        {
            Jump();
        }
        if (grounded && isJumping && ySpeed <= 0.07)
        {
            isJumping = false;
            movementSpeed = 0;
        }
    }

    public void Jump()
    {
        isJumping = true;
        ySpeed = jumpStrength;
    }

    /// <summary>
    /// Calculates gravity
    /// </summary>
    public void CalculateGravity()
    {
        
        if (!Helper.CheckBeneath(transform.position, cc, layerMask, castDistance, castScaleFactor))
        {
            ySpeed += gravity * Time.deltaTime;
            grounded = false;
        }
        else
        {
            
            grounded = true;
            if (ySpeed < 0)
            {
                ySpeed = 0;
            }
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastHitPoint = hit.point;
    }

    public void HandleLantern()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(lightController.isOn)
            {
                lightController.TurnOff();
            }
            else
            {
                lightController.TurnOn();
            }

        }
    }

    public StateName HandleTargeting()
    {
        if(lockOnState.CheckForTarget())
        {
            return StateName.LockOn;
        }
        return stateName;
    }

    #region StateMethodes
    public override void UpdateState(GameObject source)
    {
        if (isTransitioning)
        {
            isTransitioning = false;
            return;
        }
        HandleJump();
        CalculateGravity();
        Movement();
        Rotation();
        Animations();
        HandleLantern();
    }

    public override void EnterState(GameObject source)
    {
        camAnim.SetInteger("cam", 0);
    }

    public override StateName Transition(GameObject source)
    {
        if (Input.GetKey(KeyCode.LeftShift) && dashState.dashCooldownTimer < Time.time)
        {
            return StateName.Dashing;
        }
        if (Input.GetMouseButton(1))
        {
            camAnim.SetInteger("cam", 1);
            return StateName.Aiming;
        }
        if(Input.GetMouseButton(0))
        {
            return StateName.Attacking;
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            return HandleTargeting();
        }
        if(edgeDetected)
        {
            anim.SetBool("jumping", false);
            return StateName.Edgeing;
        }
        return StateName.Controlling;
    }

    public override void ExitState(GameObject source)
    {

    }

    #endregion
    void OnDrawGizmosSelected()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        for(int i = 0; i < 10;i++)
        {
            Gizmos.DrawSphere(transform.position + new Vector3(0, castDistance / i,0),(characterController.radius + characterController.skinWidth) * castScaleFactor);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.forward * edgeDetectionZ + new Vector3(0,edgeDetectionY,0), transform.position + transform.forward * edgeDetectionZ + new Vector3(0, edgeDetectionY, 0) + (Vector3.down * edgeDetectionDistance));

    }

}
