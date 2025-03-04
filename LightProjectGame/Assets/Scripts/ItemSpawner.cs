using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject item;
    public GameObject shadowEssence;
    private bool spawnHealthPotion;
    public float force = 3;
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
        GameObject _item;
        if (spawnHealthPotion)
        {

            _item = Instantiate(item, transform.position, transform.rotation);
        }
        else
        {
            _item = Instantiate(shadowEssence, transform.position, transform.rotation);
        }
        spawnHealthPotion = !spawnHealthPotion;

        Vector3 rnd = Random.onUnitSphere;
        _item.GetComponent<Rigidbody>().velocity = new Vector3(rnd.x * 1f, force, rnd.z * 1f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, transform.up);
        _item.GetComponent<Rigidbody>().velocity = rotation * _item.GetComponent<Rigidbody>().velocity;
        _item.GetComponent<Rigidbody>().angularVelocity = new Vector3(rnd.x * 1f, 7, rnd.z * 1f);
    }
}
