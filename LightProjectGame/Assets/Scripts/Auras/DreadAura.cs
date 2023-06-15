﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class DreadAura : Buff
    {
        private GameObject player;
        public List<StatusManager> enemies = new List<StatusManager>();
        private float auraRange = 10;

        public DreadAura()
        {
            buffname = BuffName.DreadAura;
        }
        public override void BuffApplication(StatusManager soruce)
        {
            
            this.soruce = soruce;
            player = GameManager.enemyTargetsInScene[0].sm.gameObject;
            enemies = new List<StatusManager>();
            foreach (StatusManager enemy in GameManager.instance.enemysInScene)
            {
                if(enemy.GetComponent<Fleeing_EnemyState>() != null)
                {
                    enemies.Add(enemy);
                }
                
            }
        }

        public override void BuffEffect()
        {
            foreach (StatusManager enemy in GameManager.instance.enemysInScene)
            {
                if(Vector3.Distance(soruce.transform.position,enemy.transform.position) < auraRange)
                {
                    if (enemy.GetComponent<Fleeing_EnemyState>() != null)
                    {
                        enemy.ApplyBuff(new Emboldened());
                    }
                }
                else
                {
                    if(enemy.activeBuffs.Exists(e => e.buffname == BuffName.Emboldened))
                    {
                        Buff buff = enemy.activeBuffs.First(e => e.buffname == BuffName.Emboldened);
                        enemy.RemoveBuff(buff);
                    }
                    
                }
            }
        }

        public override void BuffEnd()
        {
            foreach (StatusManager enemy in GameManager.instance.enemysInScene)
            {
                if (enemy.activeBuffs.Exists(e => e.buffname == BuffName.Emboldened))
                {
                    Buff buff = enemy.activeBuffs.First(e => e.buffname == BuffName.Emboldened);
                    enemy.RemoveBuff(buff);
                }
            }
        }
    }
}