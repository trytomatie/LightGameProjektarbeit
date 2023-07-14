#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyShortcuts : Editor
{
    public static float minScale = -0.5f;
    public static float maxScale = 0.5f;

    private static Rigidbody[] allRigidbodies = Object.FindObjectsOfType<Rigidbody>();
    private static List<Rigidbody> notKinematicRb = new List<Rigidbody>();
    private static List<Rigidbody> removeRB = new List<Rigidbody>();

    private static Event myEvent;

    private static bool toggleState = false;

    [MenuItem("GameObject/RotateRandom")]
    static void ToggleActivationSelection()
    {
        RecordUndoState();
        foreach (GameObject go in Selection.objects)
        {
            go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, Random.Range(0, 360), go.transform.eulerAngles.z);
            float rnd = Random.Range(minScale, maxScale);
            go.transform.localScale = go.transform.localScale + new Vector3(rnd, rnd, rnd);
        }
        //var go = Selection.activeGameObject;

    }

    [MenuItem("GameObject/TerrainAdjust")]
    static void TerrainAdjust()
    {
        foreach (GameObject go in Selection.objects)
        {
            go.GetComponent<Terrain>().terrainData.heightmapResolution = 2;
        }
        //var go = Selection.activeGameObject;

    }

    private static void RecordUndoState()
    {
        Transform[] transforms = new Transform[Selection.objects.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i] = ((GameObject)Selection.objects[i]).transform;
        }
        Undo.RecordObjects(transforms, "Making changes to object");
    }

    [MenuItem("GameObject/SimulatePhysics")]
    static void SimulatePhysics()
    {
        if(EditorApplication.isPlaying)
        {
            return;
        }
        toggleState = !toggleState;
        if (toggleState)
        {
            RecordUndoState();
            allRigidbodies = Object.FindObjectsOfType<Rigidbody>();
            Physics.autoSimulation = false; // Disable automatic physics simulation
            foreach (Rigidbody rigidbody in allRigidbodies)
            {
                if (!rigidbody.isKinematic)
                {
                    rigidbody.isKinematic = true;
                    notKinematicRb.Add(rigidbody);
                }

            }

            foreach (GameObject go in Selection.objects)
            {
                if (go.GetComponent<Rigidbody>() == null)
                {
                    removeRB.Add(go.AddComponent<Rigidbody>());
                }
                go.GetComponent<Rigidbody>().isKinematic = false;
            }
            EditorApplication.update += SimulatePhysicsObjects;
            Undo.undoRedoPerformed += FixPhysics;
            myEvent = Event.current;
        }
        else
        {
            Physics.autoSimulation = true;
            foreach (Rigidbody rigidbody in notKinematicRb)
            {
                rigidbody.isKinematic = false;
            }
            for (int i = 0; i < removeRB.Count; i++)
            {
                DestroyImmediate(removeRB[i]);
            }
            EditorApplication.update -= SimulatePhysicsObjects;
            Undo.undoRedoPerformed -= FixPhysics;
        }

    }

    [RuntimeInitializeOnLoadMethod]
    private static void FixPhysics()
    {
        if(toggleState)
        {
            Physics.autoSimulation = true;
            foreach (Rigidbody rigidbody in notKinematicRb)
            {
                rigidbody.isKinematic = false;
            }
            for (int i = 0; i < removeRB.Count; i++)
            {
                DestroyImmediate(removeRB[i]);
            }
            EditorApplication.update -= SimulatePhysicsObjects;
            toggleState = !toggleState;
        }
    }

    private static void SimulatePhysicsObjects()
    {

        Physics.Simulate(Time.fixedDeltaTime); // Step the physics simulation by the fixed time step
    }
}
#endif