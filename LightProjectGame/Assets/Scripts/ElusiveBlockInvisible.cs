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
        a_isVisible = Animator.StringToHash("isVisible");
    }

    private IEnumerator PushBlock()
    {
        float time = 0;
        vs.WakeUpAllRbs();
        while (time < 1)
        {
            vs.hitbox.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), Mathf.Clamp01(time));
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        vs.hitbox.enabled = false;

    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        vs.timeFlow = -1;
        vs.WakeUpAllRbs();
        return;
    }
    public override void UpdateState(GameObject source)
    {
 
    }

    public override StateName Transition(GameObject source)
    {
        if (vs.lightsInRange.Count > 0)
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
