using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class ElusiveBlockVisible : State
{
    public List<LightController> lightsInRange = new List<LightController>();
    public Vector3 invisblePosition;
    public Collider hitbox;

    [Header("Texture Settings")]
    public float tiling = 1;
    public Vector3 tilingOffset = new Vector3(5,0,0);
    private MaterialPropertyBlock propertyBlock;
    [HideInInspector]
    public StateMachine sm;
    private Animator anim;

    [HideInInspector]
    public float time = 1;

    public int timeFlow = -1;
    private int a_offset;

    private MeshRenderer myRenderer;


    private void Start()
    {
        anim = GetComponent<Animator>();
        sm = GetComponent<StateMachine>();
        a_offset = Animator.StringToHash("offset");
        myRenderer = GetComponent<MeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetFloat("_TextureTiling", tiling);
        propertyBlock.SetVector("_TextureOffset", tilingOffset);
        myRenderer.SetPropertyBlock(propertyBlock);
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
        RaycastHit[] rcs = Physics.SphereCastAll(transform.position, 30, Vector3.forward,0.1f);
        foreach(RaycastHit rc in rcs)
        {
            if(rc.collider.GetComponent<Rigidbody>() != null)
            {
                rc.collider.GetComponent<Rigidbody>().WakeUp();
            }
        }
    }

    private void Update()
    {
        if((time < 1 && timeFlow > 0) || (time > 0 && timeFlow < 0))
        {
            time += Time.deltaTime * timeFlow;
            time = Mathf.Clamp(time,0,0.9999f);
            anim.SetFloat(a_offset, time);
            hitbox.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), time);
            if(hitbox.transform.localScale.y < 0.05f)
            {
                hitbox.enabled = false;
            }
            else
            {
                hitbox.enabled = true;
            }
        }
        // Debug
        if(Input.GetKeyDown(KeyCode.P))
        {
            sm.ManualUpdate();
        }
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        timeFlow = 1;
        WakeUpAllRbs();
        return;
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
