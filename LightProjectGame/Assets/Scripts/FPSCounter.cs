using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private int fpsCount = 0;
    private float updateInterval =1;
    private TextMeshProUGUI fpsText;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateFPS", 0,updateInterval);
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCount++;
    }

    private void UpdateFPS()
    {
        fpsText.text = "FPS: " + fpsCount;
        fpsCount = 0;
    }
}
