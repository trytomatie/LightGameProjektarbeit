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
       
        public Buff ApplyBuff(StatusManager origin)
        {
            switch(buff)
            {
                case Buff.BuffName.DreadAura:
                    return new DreadAura(origin);
                case Buff.BuffName.Emboldened:
                    return new Emboldened(origin);
            }
            return null;
        }
    }
}