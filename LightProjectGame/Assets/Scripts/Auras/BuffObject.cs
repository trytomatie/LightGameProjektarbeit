using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(menuName = "Buffs/BuffObject")]
    public class BuffObject : ScriptableObject
    {
        public Buff.BuffName buff;
       
        public Buff ApplyBuff()
        {
            switch(buff)
            {
                case Buff.BuffName.DreadAura:
                    return new DreadAura();
                case Buff.BuffName.Emboldened:
                    return new Emboldened();
            }
            return null;
        }
    }
}