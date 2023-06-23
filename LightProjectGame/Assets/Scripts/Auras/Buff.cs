using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [Serializable]
    public class Buff 
    {
        public static float tickrate = 0.1f;
        public enum BuffName {DreadAura,Emboldened,
            LightBlight
        }
        public BuffName buffname;
        public StatusManager target;
        public float duration = 100000;

        public int originID;

        public virtual void BuffApplication(StatusManager target)
        {

        }

        public virtual void BuffEffect()
        {
            duration-= tickrate;
        }

        public virtual void BuffEnd()
        { 
        
        }
    }
}