using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject item;

    public void SpawnItems(int amount)
    {
        float time = 0.5f;
        for(int i = 0; i < amount;i++)
        {
            Invoke("SpawnItem", time);
            time += 0.1f;
        }
    }

    private void SpawnItem()
    {
        GameObject _item = Instantiate(item, transform.position, transform.rotation);
        Vector3 rnd = Random.onUnitSphere;
        _item.GetComponent<Rigidbody>().velocity = new Vector3(rnd.x * 1f, 7, rnd.z * 1f);
    }
}
