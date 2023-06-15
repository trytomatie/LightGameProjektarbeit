using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [Serializable]
    public class Buff 
    {
        public enum BuffName {DreadAura,Emboldened }
        public BuffName buffname;
        public StatusManager soruce;
        public virtual void BuffApplication(StatusManager soruce)
        {

        }

        public virtual void BuffEffect()
        {

        }

        public virtual void BuffEnd()
        { 
        
        }
    }
}