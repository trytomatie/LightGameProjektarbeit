using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCredditsTrigger : MonoBehaviour
{
    
    public void LoadEndCredits()
    {
        GameManager.instance.LoadSceneAndDestroyPlayer(3);
    }
}
