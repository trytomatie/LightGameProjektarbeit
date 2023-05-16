using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpForPlaytestScript : MonoBehaviour
{
    public Transform target;
    public GameObject pressEToPickup;
    public GameObject powerUpVFX;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,target.position) < 2)
        {
            pressEToPickup.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                target.GetComponent<WeakendControllsState>().switchToHeightendState = true;
                powerUpVFX.SetActive(true);
                Destroy(gameObject);
            }
        }
        else
        {
            pressEToPickup.SetActive(false);
        }
    }
}
