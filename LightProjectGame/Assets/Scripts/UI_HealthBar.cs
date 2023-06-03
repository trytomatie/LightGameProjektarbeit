using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Image img;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        img = GetComponent<Image>();
        if (text.text.StartsWith("MP"))
        {
            print("test");
            UpdateMP(GameObject.Find("Player").GetComponent<StatusManager>());
        }
        else
        {
            UpdateHP(GameObject.Find("Player").GetComponent<StatusManager>());
        }
       
    }
    public void UpdateHP(StatusManager sm)
    {
        float value = Remap(sm.hp, 0, sm.maxHp, 0.127f, 0.9f);
        if(value >= 0.9f)
        {
            value = 1;
        }
        img.fillAmount = value;
    }

    public void UpdateMP(StatusManager sm)
    {

        text.text = string.Format("MP: {0} / {1}", Mathf.FloorToInt(sm.LightEnergy), sm.maxLightEnergy);
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
