using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(menuName = "Voice/VoiceProfile")]
    public class VoiceProfile : ScriptableObject
    {
        public AudioClip voiceLines;
        public AudioClip deathVoice;
    }
}