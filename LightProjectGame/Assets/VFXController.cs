using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect[] vfx;
    public void TriggerVFX(int i)
    {
        vfx[i].Play();
    }
}
