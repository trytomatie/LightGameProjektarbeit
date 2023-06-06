using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightController : MonoBehaviour
{

    public List<GameObject> objectsInRange = new List<GameObject>();
    public Light lightSource;
    public LayerMask lightLayer;
    private Collider lightCollision;

    public bool isOn = true;
    // Start is called before the first frame update
    void Start()
    {
        lightLayer = LayerMask.GetMask("PuzzleElement");
        lightCollision = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PuzzleElement" || other.gameObject.tag == "Enemy")
        {
            objectsInRange.Add(other.gameObject);
            if(other.gameObject.GetComponent<ElusiveBlockVisible>() != null)
            {
                other.gameObject.GetComponent<ElusiveBlockVisible>().lightsInRange.Add(this);
                other.gameObject.GetComponent<ElusiveBlockVisible>().sm.ManualUpdate();
            }
            if(other.gameObject.GetComponent<EnemyStateVarriables>() != null)
            {
                print("test");
                other.gameObject.GetComponent<EnemyStateVarriables>().LightsInRange.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PuzzleElement" || other.gameObject.tag == "Enemy")
        {
            RemoveLightInfluence(other.gameObject);
        }
    }

    public void RemoveLightInfluence(GameObject block)
    {
        if (block.GetComponent<ElusiveBlockVisible>() != null)
        {
            block.GetComponent<ElusiveBlockVisible>().lightsInRange.Remove(this);
            block.GetComponent<ElusiveBlockVisible>().sm.ManualUpdate();
        }
        if (block.GetComponent<EnemyStateVarriables>() != null)
        {
            block.GetComponent<EnemyStateVarriables>().LightsInRange.Remove(this);
        }
        objectsInRange.Remove(block);
    }

    public void TurnOn()
    {
        lightCollision.enabled = true;

        isOn = true;
        lightSource.enabled = true;
    }

    public void TurnOff()
    {
        lightCollision.enabled = false;

        isOn = false;
        GameObject[] objectsToBeRemoved = objectsInRange.FindAll(e=> e != null).ToArray();
        foreach (GameObject go in objectsToBeRemoved)
        {
            RemoveLightInfluence(go);
        }
        lightSource.enabled = false;

    }

    private void OnDestroy()
    {
        TurnOff();
    }
}
