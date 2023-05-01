#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MyShortcuts : Editor
{
    [MenuItem("GameObject/RotateRandom")]
    static void ToggleActivationSelection()
    {
        var go = Selection.activeGameObject;
        go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, Random.Range(0, 360), go.transform.eulerAngles.z);
    }
}
#endif