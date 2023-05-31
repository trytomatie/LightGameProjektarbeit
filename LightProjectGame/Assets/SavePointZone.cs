using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointZone : MonoBehaviour
{
    public Transform savePointLocation;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && GameManager.instance.lastSavePoint != savePointLocation)
        {
            GameManager.instance.lastSavePoint = savePointLocation;
            GameManager.instance.savingUI.GetComponent<Animator>().SetTrigger("visible");
        }
    }

    private void OnDrawGizmos()
    {
        Collider col = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0, 0.5f, 0.5f, 0.1f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        Gizmos.color = new Color(0, 0.5f, 0.5f, 1f);
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }


}
