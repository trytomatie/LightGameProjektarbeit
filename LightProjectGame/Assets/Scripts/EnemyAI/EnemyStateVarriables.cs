using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateVarriables : MonoBehaviour
{
    public TargetInfo target;
    public TargetInfo[] possibleTargets;
    public float circleRange = 5;
    public float speed = 4; 
    public float aggroRange = 10;
    public float attackRange = 10;
    public float circleSpeed = 3;
    public float alertedTime = 2;
    public float verticalFieldOfView = 60;
    public float horizontalFieldOfView = 360f;
    public float eyeHeight = 1.3f;
    public LayerMask layerMask;
    public State previousState;

    // Specific State Variables
    [HideInInspector] public float ciclingStartAngle;
    [HideInInspector] public float currentCircelingAngle;
    // References
    [HideInInspector] public StatusManager statusManager;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public StateMachine stateMachine;

    // Animation
    [HideInInspector] public int animSpeedHash;
    [HideInInspector] public int animAttackHash;
    [HideInInspector] public int animAlertedHash;
    [HideInInspector] public int animMovementModeHash;
    [HideInInspector] public int animXInputHash;
    [HideInInspector] public int animYInputHash;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        statusManager = GetComponent<StatusManager>();
        stateMachine = GetComponent<StateMachine>();
        animSpeedHash = Animator.StringToHash("speed");
        animAttackHash = Animator.StringToHash("attack");
        animAlertedHash = Animator.StringToHash("alerted");
        animMovementModeHash = Animator.StringToHash("movementMode");
        animXInputHash = Animator.StringToHash("xInput");
        animYInputHash = Animator.StringToHash("yInput");
        stateMachine.Transitioning.AddListener(GetPreviousState);
        Speed = Speed;
    }


    private void GetPreviousState()
    {
        previousState = stateMachine.currentState;
    }

    public Vector3 EyePosition { get => transform.position + new Vector3(0, eyeHeight, 0); }
    public float Speed { get => speed; set 
        {
            agent.speed = value;
            speed = value; 
        } 
    }

    private void OnValidate()
    {
        agent.speed = speed;
    }

}
