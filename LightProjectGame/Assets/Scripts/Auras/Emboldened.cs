using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class Emboldened : Buff
    {
        private List<StatusManager> enemies = new List<StatusManager>();

        public Emboldened(StatusManager origin)
        {
            buffname = BuffName.Emboldened;
            this.originID = origin.GetInstanceID();
        }
        public override void BuffApplication(StatusManager soruce)
        {
            this.target = soruce;
            soruce.GetComponent<EnemyStateVarriables>().emboldend = true;
        }

        public override void BuffEffect()
        {

        }

        public override void BuffEnd()
        {
            target.GetComponent<EnemyStateVarriables>().emboldend = false;
        }
    }
}