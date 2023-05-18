using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_HealthBar : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

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
        
        text.text = string.Format("HP: {0} / {1}",sm.Hp,sm.maxHp);
    }

    public void UpdateMP(StatusManager sm)
    {

        text.text = string.Format("MP: {0} / {1}", Mathf.FloorToInt(sm.LightEnergy), sm.maxLightEnergy);
    }
}
