using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item_Potion : Item
{


    public override void PickUp()
    {
        player.sm.GetComponent<Inventory>().PotionCount++;
    }

   

}
