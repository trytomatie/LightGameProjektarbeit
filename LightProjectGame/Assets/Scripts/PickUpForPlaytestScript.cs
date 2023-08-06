using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpForPlaytestScript : MonoBehaviour
{
    public Transform target;
    public GameObject pressEToPickup;
    public GameObject powerUpVFX;
    public GameObject splineVFX;
    public GameObject weakenEnemiesPrefab;
    public UnityEvent pickUpEvent;
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
            if (Input.GetButtonDown("Interact"))
            {
                target.GetComponent<WeakendControllsState>().switchToHeightendState = true;
                Instantiate(weakenEnemiesPrefab, transform.position, transform.rotation);
                powerUpVFX.SetActive(true);
                splineVFX.SetActive(true);
                pickUpEvent.Invoke();
                Destroy(gameObject);
            }
        }
        else
        {
            pressEToPickup.SetActive(false);
        }
    }
}
