using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    protected TargetInfo player;
    protected Rigidbody rb;
    protected bool triggered = false;
    protected float timer = 0;
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
                PickUp();
            }
        }
    }

    public virtual void PickUp()
    {

    }

   

}
