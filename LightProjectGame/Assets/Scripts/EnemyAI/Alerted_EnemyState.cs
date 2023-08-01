using System.Collections;
using UnityEngine;


public class Alerted_EnemyState : State
{
    private EnemyStateMethods esm;
    private EnemyStateVarriables esv;

    private float alertedTimer = 0;

    private void Start()
    {
        esm = GetComponent<EnemyStateMethods>();
        esv = GetComponent<EnemyStateVarriables>();
        InvokeRepeating("VoiceLineTest", Random.Range(5, 10f), Random.Range(5, 10f));
    }

    private void VoiceLineTest()
    {
        FeedbackManager.PlayVoiceEnemyLine(esv.voiceProfile.GetRandomVoiceLine(),transform);
    }
    #region StateMethods
    public override void EnterState(GameObject source)
    {
        esv.anim.SetTrigger(esv.animAlertedHash);
        esv.agent.updateRotation = false;
        alertedTimer = Time.time + esv.alertedTime;
    }


    /// <summary>
    /// Gets called when current state is this state
    /// </summary>
    public override void UpdateState(GameObject source)
    {
        esm.RotateToPos(esv.target.Position);
    }

    public override void ExitState(GameObject source)
    {
        esv.agent.updateRotation = true;
    }

    public override StateName Transition(GameObject source)
    {
        if(Time.time >= alertedTimer)
        {
            return StateName.Approaching;
        }
        return stateName;
    }

    public override StateName AnyTransition(GameObject source)
    {

        return base.AnyTransition(source);
    }
    #endregion
}
