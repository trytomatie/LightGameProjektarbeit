using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class LightBlight : Buff
    {
        private LightController lc;
        private StatusManager originStatus;
        public LightBlight(StatusManager origin)
        {
            buffname = BuffName.LightBlight;
            duration = 100;
            originID = origin.gameObject.GetInstanceID();
            originStatus = origin;
        }
        public override void BuffApplication(StatusManager soruce)
        {
            this.target = soruce;
            
            lc = target.GetComponentInChildren<LightController>();
            lc.Flicker = true;
        }

        public override void BuffEffect()
        {
            if(originStatus.Hp <= 0)
            {
                duration = 0;
            }
        }

        public override void BuffEnd()
        {
            lc.Flicker = false;
        }
    }
}