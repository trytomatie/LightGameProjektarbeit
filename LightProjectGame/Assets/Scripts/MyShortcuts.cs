#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MyShortcuts : Editor
{
    public static float minScale = 0.8f;
    public static float maxScale = 1.2f;
    [MenuItem("GameObject/RotateRandom")]
    static void ToggleActivationSelection()
    {
        var go = Selection.activeGameObject;
        go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, Random.Range(0, 360), go.transform.eulerAngles.z);
        float rnd = Random.Range(minScale, maxScale);
        go.transform.localScale = new Vector3(rnd, rnd, rnd);
    }
}
#endif