using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private TargetInfo player;
    private Rigidbody rb;
    private bool triggered = false;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.enemyTargetsInScene[0];
        rb = GetComponent<Rigidbody>();
        timer = Time.time + 2f;
    }

    private void Update()
    {
        if(timer > Time.time)
        {
            return;
        }
        if(!triggered && player.Distance(transform.position) < 1)
        {
            triggered = true;
               
        }
        else if(triggered)
        {
            rb.velocity = (player.Direction(transform.position).normalized * 8);
            if(player.Distance(transform.position) < 0.15f)
            {
                Destroy(gameObject);
            }
        }
    }

   

}
