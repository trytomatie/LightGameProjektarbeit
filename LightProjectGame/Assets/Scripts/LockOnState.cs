using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LockOnState : State
{
    [Header("Targeting")]
    public StatusManager target;
    public Transform targetCameraPivot;
    public Transform targetIkPosition;
    public RectTransform targetIndicator;
    public GameObject shield;
    private NavMeshAgent targetNavMeshAgent;
    private float targetIndicatorYoffset;


    private Camera mainCamera;
    private PlayerController pc;
    private StatusManager myStatus;
    private DashingState dashState;
    private StateMachine sm;
    public RectTransform blockIndicator;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        myStatus = GetComponent<StatusManager>();
        dashState = GetComponent<DashingState>();
        pc = GetComponent<PlayerController>();
        sm = GetComponent<StateMachine>();
    }

    public bool CheckForTarget()
    {
        bool result = false;
       RaycastHit[] rcs = Physics.SphereCastAll(mainCamera.transform.position, 10, mainCamera.transform.forward, 5);
        float distance = float.PositiveInfinity;
        foreach(RaycastHit rc in rcs)
        {
            if(rc.collider.GetComponent<StatusManager>() != null)
            {

                StatusManager sm = rc.collider.GetComponent<StatusManager>();
                if (sm.faction != myStatus.faction)
                {
                    float calculatedDistance = Vector3.Distance(rc.transform.position, transform.position);
                    if (calculatedDistance < distance)
                    {
                        distance = calculatedDistance;
                        target = sm;
                        targetNavMeshAgent = sm.GetComponent<NavMeshAgent>();
                        targetIndicatorYoffset = targetNavMeshAgent.height + 0.3f;
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    private void Update()
    {
        if (target == null)
            return;
        CalculateTargetCameraPivot();
        SetTargetIndicator();
    }

    private void CalculateTargetCameraPivot()
    {
        targetCameraPivot.position = (transform.position + target.transform.position) /2;
        targetIkPosition.position = target.transform.position + new Vector3(0, 0.5f, 0);
    }

    private void SetTargetIndicator()
    {
        targetIndicator.position = mainCamera.WorldToScreenPoint(target.transform.position + new Vector3(0, targetIndicatorYoffset,0));
    }

    public void HandleShielding()
    {
        if(myStatus.ShieldHp > 0)
        {
            Shielding(true);
        }
        else
        {
            Shielding(false);
        }
    }

    public void AnimationsParemetersInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 movement= Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        float dot = Vector3.Dot(forward, movement.normalized);
        float dotRight = Vector3.Dot(right, movement.normalized);

        float maxDot = Mathf.Max(Mathf.Abs(dotRight), Mathf.Abs(dot));
        if(maxDot == Mathf.Abs(dot))
        {
            dotRight = 0;
        }
        else
        {
            dot = 0;
        }

        pc.anim.SetFloat("xInput", Mathf.RoundToInt(dotRight),0.1f,Time.deltaTime);
        pc.anim.SetFloat("yInput", Mathf.RoundToInt(dot), 0.1f, Time.deltaTime);
    }


    public void Shielding(bool value)
    {
        pc.anim.SetBool("shielding", value);
        shield.SetActive(value);
        myStatus.shielding = value;
        blockIndicator.gameObject.SetActive(value);
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        sm.states.Find(e => e.stateName == StateName.Controlling).stateName = StateName.LockOn;
        stateName = StateName.Controlling;
        pc.camAnim.SetInteger("cam", 3);
        pc.anim.SetFloat("movementMode", 1);
        HandleShielding();
        CalculateTargetCameraPivot();
        SetTargetIndicator();

        targetIndicator.gameObject.SetActive(true);
    }

    public override void UpdateState(GameObject source)
    {
        if (target == null)
            return;
        pc.Movement();
        pc.Animations();
        pc.HandleLantern();
        pc.CalculateGravity();
        pc.HandleJump();
        HandleRotation();
        AnimationsParemetersInput();
        if(myStatus.shielding)
        {
            blockIndicator.position = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        }

    }

    private void HandleRotation()
    {
        Vector3 enemyDirection = -(transform.position - target.transform.position).normalized;
        float rotation = Mathf.Atan2(pc.lastMovement.x, pc.lastMovement.z) * Mathf.Rad2Deg;
        float enemyExpectedRotation = Mathf.Atan2(enemyDirection.x, enemyDirection.z) * Mathf.Rad2Deg;
        float absoluteDifference = Mathf.Abs(rotation - enemyExpectedRotation);
        pc.lastMovement = enemyDirection;
        Rotation();
    }

    public void Rotation()
    {
        float rotation = Mathf.Atan2(pc.lastMovement.x, pc.lastMovement.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 333 * Time.deltaTime);
    }

    public override StateName Transition(GameObject source)
    {
        if(Input.GetKeyDown(KeyCode.Tab) || target == null || target.Hp <= 0)
        {
            return ReturnToControlling();
        }
        if (Input.GetMouseButton(0))
        {
            return StateName.Attacking;
        }
        if (Input.GetKey(KeyCode.LeftShift) && dashState.dashCooldownTimer < Time.time)
        {
            return StateName.Dashing;
        }
        return stateName;
    }

    private void HandleRotationForDash()
    {
        pc.Movement();
        pc.Rotation();
    }

    private StateName ReturnToControlling()
    {
        sm.states.Find(e => e.stateName == StateName.LockOn).stateName = StateName.Controlling;
        stateName = StateName.LockOn;
        pc.camAnim.SetInteger("cam", 0);
        targetIndicator.gameObject.SetActive(false);
        target = null;
        return StateName.Controlling;
    }

    public override void ExitState(GameObject source)
    {
        pc.anim.SetFloat("movementMode", 0);
        if(sm.CheckStates(this).stateName != StateName.Hurt)
        {
            Shielding(false);
        }
        HandleRotationForDash();
        pc.anim.SetFloat("xInput", 0);
        pc.anim.SetFloat("yInput", 0);
        pc.anim.SetFloat("speed", 0);
    }
    #endregion
}
