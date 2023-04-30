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
        if((other.tag == "Enemy" || other.tag == "PuzzleElement")  && !hitObjects.Contains(other.gameObject))
        {
            if(other.tag == "Enemy")
            {
                hitObjects.Add(other.gameObject);
                ApplyHitEffect(other);
            }
            if (other.tag == "PuzzleElement" && other.GetComponent<LampOn>() != null)
            {
                LampOn lamp = other.GetComponent<LampOn>();
                lamp.ChangeState();
                Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
            }
            if (other.tag == "PuzzleElement" && other.GetComponent<LeverOn>() != null)
            {
                LeverOn lever = other.GetComponent<LeverOn>();
                lever.ChangeState();
                Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
            }
        }
    }

    public void ApplyHitEffect(Collider other)
    {
        other.GetComponentInChildren<Animator>().SetTrigger("hit");
        Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
    }

}
