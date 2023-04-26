using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class ElusiveBlockInvisible : State
{
    private StateMachine sm;
    private Animator anim;
    private ElusiveBlockVisible vs;
    private int a_isVisible;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sm = GetComponent<StateMachine>();
        vs = GetComponent<ElusiveBlockVisible>();
        sm.Tick(this);
        a_isVisible = Animator.StringToHash("isVisible");
    }

    private IEnumerator PushBlock()
    {
        float time = 0;
        while(time < 1)
        {
            vs.hitbox.transform.localPosition = Vector3.Lerp(Vector3.zero, vs.invisblePosition, time);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        StartCoroutine(PushBlock());
        anim.SetBool(a_isVisible, false);
    }
    public override void UpdateState(GameObject source)
    {
 
    }

    public override StateName Transition(GameObject source)
    {
        if (vs.isVisible)
        {
            return State.StateName.Visible;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion
}
