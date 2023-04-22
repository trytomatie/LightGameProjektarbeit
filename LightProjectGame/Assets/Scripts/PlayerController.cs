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

    private bool isJumping = false;
    
    public float jumpStrength = 5;
    private Vector3 lastHitPoint;
    private Vector3 slideMovement;



    private float movementSpeed = 0;
    private float ySpeed;
    private Vector3 movement;
    private Vector3 lastMovement;
    private Vector3 dashDirection;

    [Header("Dash")]
    private bool isDashing = false;
    public float dashTime = 0.5f;
    public float dashSpeed = 30;
    public float dashCooldown = 0.5f;
    public AnimationCurve dashCurve;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;
    public Material dashMaterial;

    [Header("Attack")]
    public VisualEffect slashVFX;

    private bool isTransitioning = false;

    private CharacterController cc;
    public Animator anim;
    public Camera mainCamera;
    private Volume chaseVolume;
    public AnimationCurve attackCurve;
    public Material normalMaterial;
    public Renderer renderer;






    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        // mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;

    }




    private void LateUpdate()
    {
        if(!(ySpeed - cc.velocity.y > -10))
        {
            slideMovement = (transform.position - lastHitPoint).normalized * 0.1f;
        }
        else
        {
            slideMovement = Vector3.zero;
        }
    }

    /// <summary>
    /// Handle Animations
    /// </summary>
    private void Animations()
    {
        anim.SetFloat("speed", movementSpeed);
        anim.SetFloat("ySpeed", ySpeed / 12);
        transform.position += anim.deltaPosition;
    }

    private void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("attack", true);
        }
        else
        {
            anim.SetBool("attack", false);
        }

    }

    /// <summary>
    /// Handle Rotation
    /// </summary>
    private void Rotation()
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
    private void Movement()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float targetSpeed = runSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            targetSpeed = walkSpeed;
        }
        if(isSneaking)
        {
            targetSpeed = sneakSpeed;
        }

        // If character is landing or attacking, he cant move
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            movement = transform.forward * attackCurve.Evaluate(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
            {
                movement = new Vector3(horizontalInput * 1, 0, verticalInput * 1).normalized;
            }
            else
            {
                movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
            }
        }





        Vector3 cameraDependingMovement = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * movement;
        movementSpeed = Mathf.MoveTowards(movementSpeed, movement.magnitude * targetSpeed, acceleration * Time.deltaTime);

        if (verticalInput != 0 || horizontalInput != 0)
        {
            lastMovement = cameraDependingMovement;
        }

        if(isDashing)
        {
            lastMovement = dashDirection;
            movementSpeed = dashCurve.Evaluate(Time.time - dashTimer) * dashSpeed;
            ySpeed = 0;
        }

        cc.Move(lastMovement * movementSpeed * Time.deltaTime 
            + new Vector3(0, ySpeed, 0) * Time.deltaTime
            + slideMovement + anim.deltaPosition);
    }

    /// <summary>
    /// Handle Sneaking Functionaltiy
    /// </summary>
    private void HandleSneaking()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            isSneaking = !isSneaking;
        }
    }

    /// <summary>
    /// Handle Jump Input
    /// </summary>
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping && (grounded || (ySpeed > (-0.5f * 12))))
        {
            isJumping = true;
            ySpeed = jumpStrength;
            anim.SetTrigger("jump");
        }
        if (grounded && isJumping && ySpeed <= 0.07)
        {
            isJumping = false;
            anim.SetTrigger("land");
            movementSpeed = 0;
        }
    }

    /// <summary>
    /// Calculates gravity
    /// </summary>
    private void CalculateGravity()
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

    private void HandleDash()
    {
        if(Input.GetKey(KeyCode.LeftShift) && !isDashing &&  0 >((dashCooldownTimer + dashCooldown) - Time.time))
        {
            dashDirection = lastMovement;
            isDashing = true;
            renderer.material = dashMaterial;
            anim.SetTrigger("Dash");
            Invoke("EndDash", dashTime);
            dashTimer = Time.time;
        }

    }

    private void EndDash()
    {
        isDashing = false;
        renderer.material = normalMaterial;
        movementSpeed = 0;
        dashCooldownTimer = Time.time;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastHitPoint = hit.point;
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
        HandleSneaking();
        Movement();
        Rotation();
        Animations();
        Attack();
        HandleDash();

    }

    public override void EnterState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<InteractionHandler>().canInteract && anim.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
        {
            isSneaking = false;
            movementSpeed = 0;
            Animations();
            isTransitioning = true;
            return StateName.Interacting;
        }
        return stateName;
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

    }

}
