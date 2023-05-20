using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilitySelectTest : MonoBehaviour
{
    public Toggle t1;
    public Toggle t2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            t1.isOn = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            t2.isOn = true;
        }
    }
}
