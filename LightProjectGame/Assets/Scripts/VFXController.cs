using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect[] vfx;
    public GameObject[] hitboxes;
    public void TriggerVFX(int i)
    {
        vfx[i].Play();
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
