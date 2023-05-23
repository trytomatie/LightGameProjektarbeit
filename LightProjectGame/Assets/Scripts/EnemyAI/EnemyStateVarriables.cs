using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateVarriables : MonoBehaviour
{
    public TargetInfo target;
    public TargetInfo[] possibleTargets;
    public float circleRange = 5;
    public float aggroRange = 10;
    public float attackRange = 10;
    public float circleSpeed = 3;
    public float verticalFieldOfView = 60;
    public float horizontalFieldOfView = 360f;
    public float eyeHeight = 1.3f;
    public LayerMask layerMask;



    [HideInInspector] public StatusManager statusManager;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;

    [HideInInspector] public int animSpeedHash;
    [HideInInspector] public int animAttackHash;
    [HideInInspector] public int animAlertedHash;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        statusManager = GetComponent<StatusManager>();
        animSpeedHash = Animator.StringToHash("speed");
        animAttackHash = Animator.StringToHash("attack");
        animAlertedHash = Animator.StringToHash("alerted");
    }

    public Vector3 EyePosition { get => transform.position + new Vector3(0, eyeHeight, 0); }

}
