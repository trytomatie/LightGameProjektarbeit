using System.Collections;
using UnityEngine;


public class Dead_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;
    public GameObject wisp;

    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        esv.target.aggroList.Remove(gameObject);
        esv.anim.SetTrigger("death");
        esv.rb.isKinematic = true;
        esv.col.enabled = false;
        Destroy(gameObject, 5);
        GameManager.instance.enemysInScene.Remove(esv.statusManager);
        GameObject _go = Instantiate(wisp, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);
        _go.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1,1) * 0.4f, 2, Random.Range(-1, 1) * 0.4f);
        StartCoroutine(Dissolve());
        esv.deathParticles.Play();
        FeedbackManager.PlaySFX(esv.voiceProfile.deathVoice, transform);
    }

    private IEnumerator Dissolve()
    {
        float dissolveAmount = 0;
        while(dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * 0.3f;
            esv.renderer.material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return new WaitForEndOfFrame();
        }
        esv.renderer.material.SetFloat("_DissolveAmount", 1);

    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {

    }

    public override void ExitState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {

        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
