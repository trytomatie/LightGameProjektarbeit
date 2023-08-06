using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolvingLightLightText : MonoBehaviour
{
    private Animator anim;
    private Transform player;
    public float distanceNeededToSpawn = 10;
    [Tooltip("Despawns if player leaves if false")]
    public bool persistent = true;
    void Start()
    {
        player = GameManager.enemyTargetsInScene[0].sm.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(distanceNeededToSpawn > Vector3.Distance(transform.position,player.position))
        {
            anim.SetBool("animate", true);
            if(persistent)
            {
                this.enabled = false;
            }
        }
        else
        {
            anim.SetBool("animate", false);
        }
    }
}
