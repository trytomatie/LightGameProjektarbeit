using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<StatusManager>().ApplyDamage(1000);
        }
    }

    private void OnDrawGizmos()
    {
        Collider col = GetComponent<BoxCollider>();
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        Gizmos.color = new Color(1, 0, 0, 1);
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
