using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class VolumeEditor : MonoBehaviour
{
    public VolumeProfile[] volumeProfiles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGamma(Slider slider)
    {
        foreach(VolumeProfile vp in volumeProfiles)
        {
            LiftGammaGain lgg;
            if(vp.TryGet( out lgg))
            {
                lgg.gamma.value = new Vector4(slider.value, slider.value, slider.value, slider.value);
            }
        }
    }
}
