using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager : MonoBehaviour
{
    public enum Faction { Enemy, Player };
    public Faction faction = Faction.Enemy;

    public int maxHp;
    public int hp;

    public UnityEvent deathEvent;
    public UnityEvent damageEvent;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public int Hp
    {
        get => hp;
        set
        {
            if(value < hp)
            {
                damageEvent.Invoke();
            }
            hp = value;
            if(hp <= 0)
            {
                deathEvent.Invoke();
            }
        }
    }

}
