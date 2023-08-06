using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class VFXGraphHandler : MonoBehaviour
{
    public float duration = 5;
    internal VisualEffect vfxGraph;
    public UnityEvent followUpEvent;
    // Start is called before the first frame update
    void Start()
    {
        vfxGraph = GetComponent<VisualEffect>();
    }

    public void Play()
    {
        vfxGraph.Play();
        CancelInvoke();
        Invoke("Stop", duration);
        followUpEvent.Invoke();
    }
    public void PlayDelayed(float delay)
    {
        if(vfxGraph == null)
        {
            vfxGraph = GetComponent<VisualEffect>();
        }
        Invoke("Play", delay);
    }

    

    public void Stop()
    {
        vfxGraph.Stop();
    }

    public void FollowUpEvent()
    {
        followUpEvent.Invoke();
    }
}

#if UNITY_EDTIOR
[CustomEditor(typeof(VFXGraphHandler))]
public class VFXGraphHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VFXGraphHandler vfxGraphHandler = (VFXGraphHandler)target;
        vfxGraphHandler.vfxGraph = vfxGraphHandler.GetComponent<VisualEffect>();
        if (GUILayout.Button("Play Effect"))
        {
            vfxGraphHandler.Play();
        }
    }
}
#endif
