using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DissolveHandler : MonoBehaviour
{
    private List<LightController> lightsInScene = new List<LightController>();
    public Transform[] positions = new Transform[3];
    private Renderer meshRenderer;
    private Collider meshCollider;

    private float time = 1;
    public float radius = 3;
    private float timeScaleMultipler = 3;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        meshCollider = GetComponent<Collider>();
        foreach(LightController lc in GameObject.FindObjectsOfType<LightController>())
        {
            lightsInScene.Add(lc);
        }

        UpdateDistance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateDistance();
    }

    private void Update()
    {
        meshRenderer.material.SetVector("_LightPos1", positions[0].position);
        meshRenderer.material.SetVector("_LightPos2", positions[1].position);
        meshRenderer.material.SetVector("_LightPos3", positions[2].position);
    }

    private void UpdateDistance()
    {

        var orderedList = lightsInScene.OrderBy(x => Vector3.Distance(x.transform.position, transform.position));

        if(!orderedList.ToArray()[0].isOn)
        {
            time = Mathf.Clamp01(time - Time.deltaTime * timeScaleMultipler);
        }
        else
        {
            time = Mathf.Clamp01(time + Time.deltaTime * timeScaleMultipler);
        }
        if (time == 0)
        {
            meshCollider.enabled = true;
        }
        else
        {
            meshCollider.enabled = false;
        }
        meshRenderer.material.SetFloat("_Radius", time * radius);
        for (int i = 0; i < 3;i++)
        {
            positions[i] = orderedList.ToArray()[i].transform;
        }

    }
}

public class ValueComparer : IComparer<LightController>
{
    public int Compare(float x, float y)
    {
        return x.CompareTo(y);
    }

    public int Compare(LightController x, LightController y)
    {
        throw new System.NotImplementedException();
    }
}
