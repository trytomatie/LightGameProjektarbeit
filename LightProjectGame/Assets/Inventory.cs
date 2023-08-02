using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int potionCount;
    public TextMeshProUGUI potionText;
    private void Start()
    {
        PotionCallback(PotionCount);
    }
    public int PotionCount { get => potionCount;
        set
        {

            potionCount = value;
            PotionCallback(value);
        }
    }

    private void PotionCallback(int value)
    {
        potionText.text = "x" + value;
    }
}
