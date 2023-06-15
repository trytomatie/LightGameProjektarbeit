using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class Emboldened : Buff
    {
        private List<StatusManager> enemies = new List<StatusManager>();

        public Emboldened()
        {
            buffname = BuffName.Emboldened;
        }
        public override void BuffApplication(StatusManager soruce)
        {
            this.soruce = soruce;
            soruce.GetComponent<EnemyStateVarriables>().emboldend = true;
        }

        public override void BuffEffect()
        {

        }

        public override void BuffEnd()
        {
            soruce.GetComponent<EnemyStateVarriables>().emboldend = false;
        }
    }
}