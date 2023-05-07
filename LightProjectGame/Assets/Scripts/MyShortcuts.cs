#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MyShortcuts : Editor
{
    public static float minScale = -0.1f;
    public static float maxScale = 0.1f;
    [MenuItem("GameObject/RotateRandom")]
    static void ToggleActivationSelection()
    {
        foreach(GameObject go in Selection.objects)
        {
            go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, Random.Range(0, 360), go.transform.eulerAngles.z);
            float rnd = Random.Range(minScale, maxScale);
            go.transform.localScale = go.transform.localScale + new Vector3(rnd, rnd, rnd);
        }
        //var go = Selection.activeGameObject;

    }
}
#endif