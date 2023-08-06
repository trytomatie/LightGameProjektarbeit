using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingPlaneManager : MonoBehaviour
{
    public CinemachineFreeLook cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeFarClippingPlane(int value)
    {
        cam.m_Lens.FarClipPlane = value;
    }
}
