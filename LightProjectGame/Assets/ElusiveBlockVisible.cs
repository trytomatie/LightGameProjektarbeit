using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class ElusiveBlockVisible : State
{
    public bool isVisible = true;
    public Vector3 invisblePosition;
    public Collider hitbox;
    private StateMachine sm;
    private Animator anim;

    private int a_isVisible;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sm = GetComponent<StateMachine>();
        sm.Tick(this);
        a_isVisible = Animator.StringToHash("isVisible");
    }

    private IEnumerator PushBlock()
    {
        float time = 0;
        while(time < 1)
        {
            hitbox.transform.localPosition = Vector3.Lerp(invisblePosition,Vector3.zero, time);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isVisible = !isVisible;
            sm.ManualUpdate();
        }
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        StartCoroutine(PushBlock());
        anim.SetBool(a_isVisible, true);
    }
    public override void UpdateState(GameObject source)
    {
 
    }

    public override StateName Transition(GameObject source)
    {
        if (!isVisible)
        {
            return State.StateName.Invisible;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
