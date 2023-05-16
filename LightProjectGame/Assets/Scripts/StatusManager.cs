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

    [Header("Shielding")]
    public bool shielding = false;
    public int shieldHp = 3;
    public int shieldMaxHp = 3;

    public UnityEvent deathEvent;
    public UnityEvent damageEvent;
    public UnityEvent shieldDamageEvent;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public void ApplyDamage(int damage)
    {
        if(shielding)
        {
            ShieldHp -= damage;
        }
        else
        {
            Hp -= damage;
        }
    }

    public int Hp
    {
        get => hp;
        set
        {
            if(value < hp)
            {
                hp = value;
                damageEvent.Invoke();
            }
            if(hp <= 0)
            {
                hp = value;
                deathEvent.Invoke();
            }
        }
    }

    public int ShieldHp
    {
        get => shieldHp;
        set
        {
            if (value < shieldHp)
            {
                shieldHp = value;
                shieldDamageEvent.Invoke();
            }
            else
            {
                shieldHp = value;
            }

        }
    }

}
