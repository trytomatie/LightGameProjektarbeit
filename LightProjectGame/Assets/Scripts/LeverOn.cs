using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class LeverOn : State
{
    private StateMachine sm;
    public bool isOn = true;
    public UnityEvent eventOn;
    public Animator anim;

    private void Start()
    {
        sm = GetComponent<StateMachine>();
        sm.currentState.EnterState(gameObject);
    }

    public void ChangeState()
    {
        isOn = !isOn;
        sm.ManualUpdate();
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        eventOn.Invoke();
        anim.SetBool("animate", true);
    }
    public override void UpdateState(GameObject source)
    {

    }

    public override StateName Transition(GameObject source)
    {
        if(!isOn)
        {
            return State.StateName.LampOff;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {

    }
    #endregion

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(LeverOn))]
    public class LeverOnEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LeverOn myScript = (LeverOn)target;
            if (GUILayout.Button("Turn Off/On"))
            {
                myScript.isOn = !myScript.isOn;
                EditorUtility.SetDirty(myScript.GetComponent<StateMachine>());
                EditorUtility.SetDirty(myScript);
                if (myScript.isOn)
                {
                    myScript.GetComponent<StateMachine>().currentState = myScript.GetComponent<LeverOn>();
                    myScript.GetComponent<Animator>().SetBool("animate", true);
                }
                if (!myScript.isOn)
                {
                    myScript.GetComponent<StateMachine>().currentState = myScript.GetComponent<LeverOff>();
                    myScript.GetComponent<Animator>().SetBool("animate", false);
                }
            }
        }
    }
#endif
}
