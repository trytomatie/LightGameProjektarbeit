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
    private Animator lightSourceAnimator;

    private float lightStrength = 0;

    public bool isOn = true;
    private bool flicker = false;

    public bool Flicker { get => flicker; set
        {

            if (value != flicker)
            {
                if (value == true)
                {
                    TurnOff();
                    StartCoroutine(Flackern());
                }
            }
            flicker = value;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        lightLayer = LayerMask.GetMask("PuzzleElement");
        lightCollision = GetComponent<Collider>();
        if (lightSource.GetComponent<Animator>())
        {
            lightSourceAnimator = lightSource.GetComponent<Animator>();
            lightStrength = isOn ? 1 : 0;
            lightSourceAnimator.SetFloat("lightStrength", lightStrength);
        }
        Flicker = false;

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
        if (Flicker) return;

        lightCollision.enabled = true;
        isOn = true;
        if(lightSourceAnimator != null)
        {
            StopAllCoroutines();
            StartCoroutine(Light(1));
        }
        else
        {
            lightSource.enabled = true;
        }

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
        if (lightSourceAnimator != null)
        {
            if (Flicker) return;

            StopAllCoroutines();
            StartCoroutine(Light(-1));

        }
        else
        {
            lightSource.enabled = false;
        }

    }

    private IEnumerator Light(float direction)
    {
        do
        {
            lightStrength += direction * Time.deltaTime * 3;
            lightSourceAnimator.SetFloat("lightStrength",lightStrength);
            yield return new WaitForEndOfFrame();
        }
        while (lightStrength >= 0 && lightStrength <= 1);
        lightStrength = Mathf.Clamp01(lightStrength);
        lightSourceAnimator.SetFloat("lightStrength", lightStrength);
    }

    private IEnumerator Flackern()
    {
        float time = 0;
        do
        {
            lightStrength = Random.Range(0f, 0.5f);
            lightSourceAnimator.SetFloat("lightStrength", lightStrength);
            yield return new WaitForSeconds(Random.Range(0, 0.15f));
            if(time % 30 == 15)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 0.7f));
            }
            time++;
        }
        while (time < 1000000 && Flicker == true);
        lightStrength = 0;
        lightSourceAnimator.SetFloat("lightStrength", lightStrength);
        TurnOn();
    }

    private void OnDestroy()
    {
        TurnOff();
    }
}
