using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect[] vfx;
    public GameObject[] hitboxes;


    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.K))
        {
            hitboxes[0].SetActive(true);
            foreach(GameObject go in hitboxes[0].GetComponent<DamageObject>().hitObjects)
            {
                hitboxes[0].GetComponent<DamageObject>().ApplyHitEffect(go.GetComponent<Collider>());
            }
            hitboxes[0].transform.position = hitboxes[0].transform.position + Random.insideUnitSphere * 0.001f;
            TriggerVFXSpam(0);
        }
    }

    public void TriggerVFX(int i)
    {
        VisualEffect _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation,vfx[i].transform.parent);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.parent = null;
        Destroy(_vfx, 10f);
        //vfx[i].Play();
    }
    public void TriggerVFXSpam(int i)
    {
        VisualEffect _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        //vfx[i].Play();
    }

    public void TriggerHitbox(int i)
    {
        hitboxes[i].SetActive(true);
    }

    public void UnTriggerHitbox(int i)
    {
        hitboxes[i].SetActive(false);
    }
}
