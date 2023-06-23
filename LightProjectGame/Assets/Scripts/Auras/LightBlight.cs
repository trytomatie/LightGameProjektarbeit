using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class LightBlight : Buff
    {
        private LightController lc;
        public LightBlight(StatusManager origin)
        {
            buffname = BuffName.LightBlight;
            duration = 100;
            originID = origin.gameObject.GetInstanceID();
        }
        public override void BuffApplication(StatusManager soruce)
        {
            this.target = soruce;
            lc = target.GetComponentInChildren<LightController>();
            lc.Flicker = true;
        }

        public override void BuffEffect()
        {

        }

        public override void BuffEnd()
        {
            lc.Flicker = false;
        }
    }
}