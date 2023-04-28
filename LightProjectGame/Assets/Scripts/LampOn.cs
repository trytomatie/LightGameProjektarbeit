using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class LampOn : State
{
    public LightController lc;
    private StateMachine sm;
    public bool isOn = true;



    private void Start()
    {
        sm = GetComponent<StateMachine>();
    }

    public void ChangeState()
    {
        isOn = !isOn;
        sm.ManualUpdate();
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        lc.TurnOn();
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
    [UnityEditor.CustomEditor(typeof(LampOn))]
    public class LampOnEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LampOn myScript = (LampOn)target;
            if (GUILayout.Button("Turn Off/On"))
            {
                EditorUtility.SetDirty(myScript.GetComponent<StateMachine>());
                EditorUtility.SetDirty(myScript.lc.lightSource);
                EditorUtility.SetDirty(myScript);
                myScript.ChangeStateEditor();
            }
        }
    }

    public void ChangeStateEditor()
    {
        isOn = !isOn;
        if(isOn)
        {
            GetComponent<StateMachine>().currentState = this;
            lc.lightSource.enabled = true;
            lc.GetComponent<Collider>().enabled = true;
            lc.isOn = true;
        }
        else
        {
            GetComponent<StateMachine>().currentState = GetComponent<LampOff>();
            lc.lightSource.enabled = false;
            lc.GetComponent<Collider>().enabled = false;
            lc.isOn = false;
            if (UnityEditor.EditorApplication.isPlaying)
            {
                GameObject[] objectsToBeRemoved = lc.objectsInRange.ToArray();
                foreach (GameObject go in objectsToBeRemoved)
                {
                    lc.RemoveLightInfluence(go);
                }
            }
        }
    }
#endif
}
