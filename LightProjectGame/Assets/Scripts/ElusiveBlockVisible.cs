using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class ElusiveBlockVisible : State
{
    public List<LightController> lightsInRange = new List<LightController>();
    public Vector3 invisblePosition;
    public Collider hitbox;
    [HideInInspector]
    public StateMachine sm;
    private Animator anim;

    private int a_isVisible;


    private void Start()
    {
        anim = GetComponent<Animator>();
        sm = GetComponent<StateMachine>();
        a_isVisible = Animator.StringToHash("isVisible");
    }


    private IEnumerator PushBlock()
    {
        float time = 0;
        WakeUpAllRbs();
        while (time <= 1)
        {
            time += Time.deltaTime;
            hitbox.transform.localScale = Vector3.Lerp(new Vector3(1,0,1),new Vector3(1,1,1), Mathf.Clamp01(time));
            yield return new WaitForEndOfFrame();
        }


    }

    public void WakeUpAllRbs()
    {
        RaycastHit[] rcs = Physics.SphereCastAll(transform.position, 3, Vector3.forward,0.1f);
        foreach(RaycastHit rc in rcs)
        {
            if(rc.collider.GetComponent<Rigidbody>() != null)
            {
                print(rc.collider.gameObject.name);
                rc.collider.GetComponent<Rigidbody>().WakeUp();
            }
        }
    }

    private void Update()
    {
        // Debug
        if(Input.GetKeyDown(KeyCode.P))
        {
            sm.ManualUpdate();
        }
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        hitbox.enabled = true;
        StartCoroutine(PushBlock());
        anim.SetBool(a_isVisible, true);
    }
    public override void UpdateState(GameObject source)
    {
 
    }

    public override StateName Transition(GameObject source)
    {
        if (lightsInRange.Count <= 0)
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
