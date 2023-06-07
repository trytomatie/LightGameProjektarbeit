using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DissolveHandler : MonoBehaviour
{
    public Transform[] positions = new Transform[3];
    public LightController playerLight;
    private Renderer meshRenderer;
    private Collider meshCollider;

    private float time = 1;
    public float radius = 3;
    private float timeScaleMultipler = 3;

    private int lightSourcesInScene = 0;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        meshCollider = GetComponent<Collider>();
        GameObject player = GameObject.Find("Player");
        positions[0] = player.transform;
        playerLight = player.GetComponentInChildren<LightController>();


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
    }

    private void UpdateDistance()
    {

        if(!playerLight.isOn)
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
