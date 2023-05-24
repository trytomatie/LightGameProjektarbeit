using System;
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

    public bool isInvulnerable = false;

    [Header("Shielding")]
    public bool shielding = false;
    public int shieldHp = 3;
    public int shieldMaxHp = 3;

    [Header("Speed")]
    public float moveSpeed = 3;


    [Header("LightEnergy")]
    public float lightEnergy = 0;
    public float maxLightEnergy = 0;
    private bool hasCast = false;

    public UnityEvent deathEvent;
    public UnityEvent damageEvent;
    public UnityEvent lightEnergyEvent;
    public UnityEvent shieldDamageEvent;

    [HideInInspector]
    public TargetInfo targetInfo;

    // Start is called before the first frame update
    void Awake()
    {
        hp = maxHp;
        targetInfo = new TargetInfo(gameObject);
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



    public void RestoreMinimumLightEnergy()
    {
        hasCast = false;
        StartCoroutine(RegenLightEnergy());
        CancelInvoke();
    }

    public IEnumerator RegenLightEnergy()
    {
        while(lightEnergy < 10 && !hasCast)
        {
            yield return new WaitForEndOfFrame();
            LightEnergy += Time.deltaTime * 2;
        }
    }

    public float LightEnergy
    {
        get => lightEnergy;
        set
        {
            if (value < lightEnergy)
            {
                lightEnergy = value;
                hasCast = true;
                if (lightEnergy < 10)
                {
                    Invoke("RestoreMinimumLightEnergy", 5);
                }
            }
            else
            {
                lightEnergy = value;
                if(lightEnergy > maxLightEnergy)
                {
                    lightEnergy = maxLightEnergy;
                }
            }
            lightEnergyEvent.Invoke();
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

public class TargetInfo
{
    private Vector3 position;
    public List<GameObject> aggroList = new List<GameObject>();
    public StatusManager sm;

    public Vector3 Position 
    { 
        get => sm.transform.position;
    }

    public TargetInfo(GameObject gameObject)
    {
        sm = gameObject.GetComponent<StatusManager>();
    }
    
    public float Distance(Vector3 pos)
    {
        return Vector3.Distance(pos, Position);
    }

    public bool HasLoS(Vector3 pos,float sightDistance,LayerMask layerMask)
    {
        return Physics.Raycast(new Ray(pos, Direction(pos)), sightDistance, layerMask);
    }

    public Vector3 Direction(Vector3 pos)
    {
        return Position - pos;
    }

    public void MoveInAggroList(int targetIndex,int direction)
    {
        if (aggroList == null)
        {
            throw new ArgumentNullException(nameof(aggroList));
        }

        if (targetIndex < 0 || targetIndex >= aggroList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(aggroList));
        }

        if (direction == 0)
        {
            return; // No movement required
        }

        int newIndex = targetIndex + direction;

        if (newIndex < 0)
        {
            newIndex = 0;
        }

        if(newIndex >= aggroList.Count)
        {
            newIndex = aggroList.Count - 1;
        }

        GameObject item = aggroList[targetIndex];
        aggroList.RemoveAt(targetIndex);
        aggroList.Insert(newIndex, item);
    }
}
