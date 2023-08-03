using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class CutszeneState : State
{
    private PlayerController pc;
    public bool cutszeneFinished = false;
    public bool startCutszene = false;
    private float timer = 0;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        pc.camAnim.SetInteger("cam", 4);
        GameManager.instance.canvas.SetActive(false);
        GameManager.instance.currentCutszene.Play();
        startCutszene = false;
        timer = Time.time + (float)GameManager.instance.currentCutszene.duration;
    }
    public override void UpdateState(GameObject source)
    {
        pc.CalculateGravity();
        pc.Animations();
        if(Time.time > timer)
        {
            cutszeneFinished = true;
        }
    }

    public override StateName Transition(GameObject source)
    {
        if (cutszeneFinished)
        {
            return StateName.Controlling;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        pc.camAnim.SetInteger("cam", 0);
        GameManager.instance.canvas.SetActive(true);
    }

    public override StateName AnyTransition(GameObject source)
    { 
        if(startCutszene)
        {
            return StateName.Cutszene;
        }
        return base.AnyTransition(source);
    }
    #endregion
}
