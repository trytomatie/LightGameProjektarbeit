using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyAnchor : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 startPos;
    private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(parent.transform.position + startPos);
    }
}
