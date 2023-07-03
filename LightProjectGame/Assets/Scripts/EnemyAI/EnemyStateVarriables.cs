using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateVarriables : MonoBehaviour
{
    public TargetInfo target;
    public TargetInfo[] possibleTargets;
    public List<LightController> lightsInRange = new List<LightController>();
    public float circleRange = 5;
    public float speed = 4; 
    public float aggroRange = 10;
    public Vector2 attackIntervalls = new Vector2(1, 7);
    public float attackRange = 10;
    public float circleSpeed = 3;
    public float alertedTime = 2;
    public float verticalFieldOfView = 60;
    public float horizontalFieldOfView = 360f;
    public bool ignoreLoS = false;
    public float eyeHeight = 1.3f;
    public LayerMask layerMask;
    public State previousState;
    public bool emboldend;

    public float[] aggrolistPositionModifier = { 1, 0f, 0f,0f };
    public float[] aggroListAttackRollPenalty = { 0f, 0.5f, 0.8f, 1f };

    public ParticleSystem deathParticles;

    // Specific State Variables
    [HideInInspector] public float ciclingStartAngle;
    [HideInInspector] public float currentCircelingAngle;
    // References
    [HideInInspector] public StatusManager statusManager;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Renderer renderer;

    // Animation
    [HideInInspector] public int animSpeedHash;
    [HideInInspector] public int animAttackHash;
    [HideInInspector] public int animAlertedHash;
    [HideInInspector] public int animMovementModeHash;
    [HideInInspector] public int animXInputHash;
    [HideInInspector] public int animYInputHash;
    [HideInInspector] public int animTauntHash;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        renderer = GetComponentInChildren<Renderer>();
        statusManager = GetComponent<StatusManager>();
        stateMachine = GetComponent<StateMachine>();
        animSpeedHash = Animator.StringToHash("speed");
        animAttackHash = Animator.StringToHash("attack");
        animAlertedHash = Animator.StringToHash("alerted");
        animMovementModeHash = Animator.StringToHash("movementMode");
        animXInputHash = Animator.StringToHash("xInput");
        animYInputHash = Animator.StringToHash("yInput");
        animTauntHash = Animator.StringToHash("taunt");
        stateMachine.Transitioning.AddListener(GetPreviousState);
        GameManager.instance.enemysInScene.Add(statusManager);
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

    public float[] AggrolistPositionModifier { get => aggrolistPositionModifier; }
    public float[] AggroListAttackRollPenalty { get => aggroListAttackRollPenalty; }
    public List<LightController> LightsInRange 
    { 
        get 
        {
            return lightsInRange;
        }
        set => lightsInRange = value; 
    }

    private void OnValidate()
    {
        GetComponent<NavMeshAgent>().speed = speed;
    }

    private void OnDestroy()
    {
        GameManager.instance.enemysInScene.Remove(statusManager);
        target.aggroList.Remove(gameObject);
    }

}
