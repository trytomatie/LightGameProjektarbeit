using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilitySelectTest : MonoBehaviour
{
    public GameObject[] images;
    private static UI_AbilitySelectTest instance;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {

        if(instance == null)
        {
            instance = this;
            player = FindObjectOfType<PlayerController>();
            images[0].SetActive(false);
            images[1].SetActive(true);
            images[2].SetActive(true);
            images[3].SetActive(false);
        }
        else
        {
            Destroy(this);
        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void UpdateSkillUI()
    {
        if (instance.player.selectedSkill == 2)
        {
            instance.images[0].SetActive(true);
            instance.images[1].SetActive(false);
            instance.images[2].SetActive(false);
            instance.images[3].SetActive(true);

        }
        if (instance.player.selectedSkill == 1)
        {
            instance.images[0].SetActive(false);
            instance.images[1].SetActive(true);
            instance.images[2].SetActive(true);
            instance.images[3].SetActive(false);
        }
    }

    
}
