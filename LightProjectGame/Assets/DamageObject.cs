using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{

    public GameObject hitEffect;
    public List<GameObject> hitObjects;

    // Start is called before the first frame update
    void Start()
    {
        hitObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        hitObjects = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && !hitObjects.Contains(other.gameObject))
        {
            other.GetComponentInChildren<Animator>().SetTrigger("hit");
            hitObjects.Add(other.gameObject);
            Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
