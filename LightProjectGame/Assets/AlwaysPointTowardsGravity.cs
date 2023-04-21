using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPointTowardsGravity : MonoBehaviour
{
    public Vector3 targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = transform.localEulerAngles.y;
        transform.eulerAngles = new Vector3(targetRotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
       //s transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
        print(transform.eulerAngles);
    }
}
