using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Cinemachine;

[RequireComponent(typeof(StateMachine))]
public class WeakendControllsState : State
{
    private PlayerController pc;
    public GameObject lampModel;
    public LightController lc;
    private StatusManager sm;
    public GameObject[] hitBoxes;
    private DashingState dashState;

    [Header("WeaponsOnCharacter")]
    public GameObject mainWeapon;
    public GameObject[] brokenWeapons;
    public GameObject[] vfx;

    public bool switchToHeightendState = false;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        sm = GetComponent<StatusManager>();
        dashState = GetComponent<DashingState>();
    }



    #region StateMethods
    public override void EnterState(GameObject source)
    {
        lc.TurnOff();
        lampModel.SetActive(false);
        sm.shieldHp = 0;
        sm.shieldMaxHp = 0;

        foreach(GameObject hitbox in hitBoxes)
        {
            if(hitbox.GetComponent<DestroyWeaponOnHit>() == null)
            {
                DestroyWeaponOnHit d = hitbox.AddComponent<DestroyWeaponOnHit>();
                d.currentWeapon = mainWeapon;
                d.brokenWeaponParts = brokenWeapons;
                d.ws = this;
            }

        }
    }
    public override void UpdateState(GameObject source)
    {
        if (pc.isTransitioning)
        {
            pc.isTransitioning = false;
            return;
        }
        pc.HandleJump();
        pc.CalculateGravity();
        pc.Movement();
        pc.Rotation();
        pc.Animations();
    }

    public override StateName Transition(GameObject source)
    {
        if (Input.GetKey(KeyCode.LeftShift) && dashState.dashCooldownTimer < Time.time)
        {
            return StateName.Dashing;
        }
        if (Input.GetMouseButton(1))
        {
            pc.camAnim.SetInteger("cam", 1);
            return StateName.Controlling;
        }
        else
        {
            pc.camAnim.SetInteger("cam", 0);
        }
        if (Input.GetMouseButton(0))
        {
            return StateName.Attacking;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            return pc.HandleTargeting();
        }
        if (pc.edgeDetected)
        {
            pc.anim.SetBool("jumping", false);
            return StateName.Edgeing;
        }
        if(switchToHeightendState)
        {
            lc.TurnOn();
            switchToHeightendState = false;
            lampModel.SetActive(true);
            sm.shieldHp = 30;
            sm.shieldMaxHp = 30;
            foreach (GameObject hitbox in hitBoxes)
            {
                Destroy(hitbox.GetComponent<DestroyWeaponOnHit>());
                hitbox.GetComponent<DamageObject>().enabled = true;
                hitbox.GetComponent<CinemachineCollisionImpulseSource>().enabled = true;
                hitbox.GetComponent<DamageObject>().isActive = true;
            }
            foreach (GameObject v in vfx)
            {
                v.SetActive(true);
            }
            mainWeapon.SetActive(true);
            brokenWeapons[0].SetActive(false);
            stateName = StateName.Invalid;
            pc.stateName = StateName.Controlling;
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion


}
