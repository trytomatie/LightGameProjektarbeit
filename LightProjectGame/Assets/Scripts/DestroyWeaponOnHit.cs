using Cinemachine;
using System.Collections;
using UnityEngine;

public class DestroyWeaponOnHit : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject[] brokenWeaponParts;
    public WeakendControllsState ws;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            currentWeapon.SetActive(false);
            brokenWeaponParts[0].SetActive(true);
            brokenWeaponParts[1].SetActive(true);
            brokenWeaponParts[1].transform.parent = null;
            foreach (GameObject hitbox in ws.hitBoxes)
            {
                hitbox.GetComponent<DamageObject>().enabled = false;
                hitbox.GetComponent<CinemachineCollisionImpulseSource>().enabled = false;
                hitbox.GetComponent<DamageObject>().isActive = false;
            }
            foreach (GameObject v in ws.vfx)
            {
                v.GetComponent<MeshRenderer>().enabled = false;
            }
            GameObject.Find("WaffeZerbrochen_UI").GetComponent<Animator>().SetTrigger("Animate");
        }
    }
}