using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispPickupTemp : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<StatusManager>().LightEnergy += 10;
            Destroy(gameObject);
        }
    }
}
