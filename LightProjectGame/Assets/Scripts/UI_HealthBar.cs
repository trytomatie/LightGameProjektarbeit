using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Image img;
    public bool isMP;

    private void Start()
    {
        img = GetComponent<Image>();
        if (isMP)
        {

            UpdateMP(GameObject.Find("Player").GetComponent<StatusManager>());
        }
        else
        {
            UpdateHP(GameObject.Find("Player").GetComponent<StatusManager>());
        }
       
    }
    public void UpdateHP(StatusManager sm)
    {
        float value = Remap(sm.hp, 0, sm.maxHp, 0.037f, 0.9f);
        if(value >= 0.9f)
        {
            value = 1;
        }
        img.fillAmount = value;
    }

    public void UpdateMP(StatusManager sm)
    {

        float value = Remap(sm.lightEnergy, 0, sm.maxLightEnergy, 0, 1f);
        img.fillAmount = value;
    }

    public static float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        // Normalize the input value
        float normalizedValue = (value - inputMin) / (inputMax - inputMin);

        // Remap the normalized value to the output range
        float remappedValue = (normalizedValue * (outputMax - outputMin)) + outputMin;

        return remappedValue;
    }
}
