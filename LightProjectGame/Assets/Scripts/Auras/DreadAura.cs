using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class DreadAura : Buff
    {
        private StatusManager player;
        public List<StatusManager> enemies = new List<StatusManager>();
        private float auraRange = 8;
        private List<StatusManager> influencedEnemies = new List<StatusManager>();

        public DreadAura(StatusManager origin)
        {
            buffname = BuffName.DreadAura;
            this.originID = origin.GetInstanceID();
            duration = 100000;
        }
        public override void BuffApplication(StatusManager soruce)
        {
            
            this.target = soruce;
            player = GameManager.enemyTargetsInScene[0].sm;
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
                
                if(Vector3.Distance(target.transform.position,enemy.transform.position) < auraRange)
                {
                    if (enemy.GetComponent<Fleeing_EnemyState>() != null && !enemy.activeBuffs.Exists(e => e.buffname == BuffName.Emboldened))
                    {
                        enemy.ApplyBuff(new Emboldened(target));
                        influencedEnemies.Add(enemy);
                    }
                }
                else
                {
                    if(influencedEnemies.Contains(enemy) && enemy.activeBuffs.Exists(e => e.buffname == BuffName.Emboldened))
                    {
                        Buff buff = enemy.activeBuffs.First(e => e.buffname == BuffName.Emboldened);
                        influencedEnemies.Remove(enemy);
                        enemy.RemoveBuff(buff);
                    }
                    
                }
            }
            if(Vector3.Distance(target.transform.position,player.transform.position) < auraRange)
            {
                player.ApplyBuff(new LightBlight(target));
            }
            else
            {
                player.RemoveBuff(new LightBlight(target));
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
            player.RemoveBuff(new LightBlight(target));
        }
    }
}