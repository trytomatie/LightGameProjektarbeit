using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{

    public StatusManager.Faction faction = StatusManager.Faction.Player;
    public StatusManager source;
    public GameObject hitEffect;
    public List<GameObject> hitObjects;
    public int damage = 1;
    public bool isActive = true;

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
        if(!isActive)
        {
            return;
        }
        if((other.GetComponent<StatusManager>() != null || other.tag == "PuzzleElement")  && !hitObjects.Contains(other.gameObject))
        {
            if(other.GetComponent<StatusManager>() != null)
            {
                StatusManager sm = other.GetComponent<StatusManager>();
                if(sm.faction != faction && !sm.isInvulnerable)
                {
                    GameManager.CallHitPause();
                    sm.ApplyDamage(damage);
                    hitObjects.Add(other.gameObject);
                    ApplyHitEffect(other);
                    if(sm.faction == StatusManager.Faction.Player)
                    {
                        Vector3 dir = transform.position - other.transform.position;
                        dir.y = 0;
                        other.GetComponent<PlayerController>().ApplyKnockback(-dir.normalized * 5, 0.3f);
                    }
                    else
                    {
                        other.GetComponent<EnemyStateVarriables>().target = source.targetInfo;
                    }
                }
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
        if (other.GetComponent<HurtState>() != null)
        {
            other.GetComponent<HurtState>().isHit = true;
        }
        Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
    }

}
