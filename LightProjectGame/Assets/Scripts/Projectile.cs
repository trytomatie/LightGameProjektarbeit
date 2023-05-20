using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1;
    public float downForceTime = 0.5f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("BulletDrop", downForceTime);
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * speed,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BulletDrop()
    {
        rb.useGravity = true;
    }
}
