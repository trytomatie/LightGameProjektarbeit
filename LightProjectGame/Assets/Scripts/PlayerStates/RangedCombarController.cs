using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombarController : State
{
    private PlayerController playerController;
    private LockOnState lockOnState;
    private Camera mainCamera;
    public GameObject projectile;
    public Vector3 shootOffset = new Vector3(0,0.6f,0);
    public LayerMask layerMask;
    public GameObject reticle;
    public Transform ikTarget;
    public GameObject chargeAttackVFX;
    public Transform staffTip;
    public float reloadTime = 0.5f;
    public GameObject shootHud;
    public GameObject magentHud;
    private float reloadTimer = 0;
    private bool isReloading = false;


    private StateMachine sm;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mainCamera = Camera.main;
        lockOnState = GetComponent<LockOnState>();
        sm = GetComponent<StateMachine>();
    }

    void Rotation()
    {
        float rotation = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 360 * Time.deltaTime);
    }

    public void HandleHud(bool value)
    {
        shootHud.SetActive(false);
        magentHud.SetActive(false);
        if (value)
        {
            switch (playerController.selectedSkill)
            {
                case 1:
                    shootHud.SetActive(true);
                    break;
                case 2:
                    magentHud.SetActive(true);
                    break;
                default:
                    shootHud.SetActive(true);
                    break;
            }
        }
    }

    void HandleShooting()
    {
        if(Input.GetButtonDown("Shoot") && playerController.myStatus.LightEnergy>= 5 && !isReloading)
        {
            playerController.myStatus.LightEnergy -= 5;
            RaycastHit raycastHit;

            //if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 50, layerMask))
            //{
            //    Vector3 direction = (transform.position + shootOffset) - raycastHit.point;
            //    GameObject _projectile = Instantiate(projectile, transform.position + shootOffset, Quaternion.identity);
            //    _projectile.transform.LookAt(raycastHit.point);
            //}
            //else
            //{
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            GameObject _projectile = Instantiate(projectile, staffTip.transform.position, Quaternion.identity); 
            _projectile.transform.LookAt(ray.GetPoint(15));
            _projectile.GetComponent<DamageObject>().source = playerController.myStatus;
            chargeAttackVFX.SetActive(false);
            FeedbackManager.PlaySound(FeedbackManager.instance.shoot_Feedback, transform);
            isReloading = true;
            Invoke("ReadyShot", reloadTime);
            //}
        }
    }

    private void ReadyShot()
    {
        if (playerController.myStatus.LightEnergy < 5)
        {
            Invoke("ReadyShot", 1);
            return;
        }
        if (sm.CheckStates(sm.currentState) == this)
        {
            FeedbackManager.PlaySound(FeedbackManager.instance.shootCharge_Feedback, transform);
            chargeAttackVFX.SetActive(true);
        }
        Invoke("ReloadingFinish", 0.5f);
    }

    private void ReloadingFinish()
    {
        isReloading = false;
    }

    private void SetIKTargetPosition()
    {
        ikTarget.position = mainCamera.transform.position + mainCamera.transform.forward * 10;
    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 1);
        reticle.SetActive(true);
        playerController.anim.SetFloat("movementMode", 1);
        playerController.anim.SetBool("aiming", true);
        ReadyShot();
        HandleHud(true);
    }

    public override void UpdateState(GameObject source)
    {
        playerController.Movement();
        Rotation();
        playerController.Animations();
        playerController.CalculateGravity();
        HandleShooting();
        playerController.HandleLantern();
        lockOnState.AnimationsParemetersInput();
        SetIKTargetPosition();
    }

    public override StateName Transition(GameObject source)
    {
        if (!PlayerController.IsAiming())
        {
            return StateName.Controlling;
        }
        if(playerController.selectedSkill == 2)
        {
            return StateName.LookForDraggables;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        playerController.anim.SetFloat("movementMode", 0);
        playerController.anim.SetBool("aiming", false);
        reticle.SetActive(false);
        chargeAttackVFX.SetActive(false);
        HandleHud(false);
    }
    #endregion
}
