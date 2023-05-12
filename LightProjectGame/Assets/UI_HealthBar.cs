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
        UpdateHP(GameObject.Find("Player").GetComponent<StatusManager>());
    }
    public void UpdateHP(StatusManager sm)
    {
        
        text.text = string.Format("HP: {0} / {1}",sm.Hp,sm.maxHp);
    }
}
