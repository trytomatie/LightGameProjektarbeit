using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class VolumeEditor : MonoBehaviour
{
    public VolumeProfile[] volumeProfiles;
    private Slider s;
    private Exposure exposure;

    private void Start()
    {
        // Get the Exposure component from the HDRP Volume
        volumeProfiles[0].TryGet(out exposure);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGamma(Slider slider)
    {
        foreach(VolumeProfile vp in volumeProfiles)
        {
            // Map the slider value to the desired exposure range
            float exposureValue = -slider.value;

            // Set the exposure value
            exposure.fixedExposure.value = exposureValue;
        }
    }
}
