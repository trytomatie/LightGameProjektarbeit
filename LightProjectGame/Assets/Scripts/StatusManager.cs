using Assets.Scripts.Auras;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool shielding = false;
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

    public List<Buff> activeBuffs;
    private List<Buff> buffsToadd = new List<Buff>();
    private List<Buff> buffsToRemove = new List<Buff>();
    public BuffObject buffHolder;

    public TargetInfo targetInfo;

    // Start is called before the first frame update
    void Awake()
    {
        hp = maxHp;
        targetInfo = new TargetInfo(gameObject);
        
        
    }


    private void Start()
    {
        StartCoroutine(InitBuffs());
        InvokeRepeating("UpdateBuffs", 0, Buff.tickrate);
    }


    public void ApplyDamage(int damage)
    {
        if(Shielding)
        {
            ShieldHp -= damage;
        }
        else
        {
            Hp -= damage;
        }
    }

    private IEnumerator InitBuffs()
    {
        if(buffHolder != null)
        {
            while (GameManager.instance == null)
            {
                yield return new WaitForEndOfFrame();
            }
            activeBuffs.Add(buffHolder.ApplyBuff(this));
            foreach (Buff buff in activeBuffs)
            {
                buff.BuffApplication(this);
            }
        }

    }

    public void UpdateBuffs()
    {
        if (hp <= 0)
            return;
        if (buffsToadd.Count > 0)
        {
            foreach (Buff newBuff in buffsToadd)
            {
                activeBuffs.Add(newBuff);
                newBuff.BuffApplication(this);
            }
            buffsToadd.Clear();
        }
        foreach (Buff buff in activeBuffs)
        {
            buff.BuffEffect();
            buff.duration -= Buff.tickrate;
            if (buff.duration <= 0)
            {
                buffsToRemove.Add(buff);
            }
        }
        if (buffsToRemove.Count > 0)
        {
            foreach (Buff newBuff in buffsToRemove)
            {
                activeBuffs.Remove(newBuff);
                newBuff.BuffEnd();
            }
            buffsToRemove.Clear();
        }
    }

    public void ApplyBuff(Buff buff)
    {
        if(!activeBuffs.Any(e=> e.buffname == buff.buffname))
        {
            buffsToadd.Add(buff);
        }
    }

    public void RemoveBuff(Buff buff)
    {
        if (activeBuffs.Any(e => e.buffname == buff.buffname && e.originID == buff.originID))
        {
            buffsToRemove.Add(activeBuffs.First(e => e.buffname == buff.buffname));
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

    private void OnDestroy()
    {
        foreach(Buff buff in activeBuffs)
        {
            buff.BuffEnd();
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
            if(value != hp)
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
                FeedbackManager.PlaySound(FeedbackManager.instance.successfulBlock_Feedback, transform);
            }
            else
            {
                shieldHp = value;
            }

        }
    }

    public bool Shielding { get => shielding; 
        set
        {
            FeedbackManager.PlaySound(FeedbackManager.instance.readyShield_Feedback, transform);
            shielding = value;
        }
    }
}

[System.Serializable]
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
            return;
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
