using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilitySelectTest : MonoBehaviour
{
    public GameObject[] images;
    // Start is called before the first frame update
    void Start()
    {
        images[0].SetActive(false);
        images[1].SetActive(true);
        images[2].SetActive(true);
        images[3].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            images[0].SetActive(true);
            images[1].SetActive(false);
            images[2].SetActive(false);
            images[3].SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            images[0].SetActive(false);
            images[1].SetActive(true);
            images[2].SetActive(true);
            images[3].SetActive(false);
        }
    }
}
