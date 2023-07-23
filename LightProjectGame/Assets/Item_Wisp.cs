using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item_Wisp : Item
{


    public override void PickUp()
    {
        player.sm.LightEnergy += 10;
    }

   

}
