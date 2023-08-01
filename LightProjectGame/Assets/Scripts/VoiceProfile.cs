using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Voice/VoiceProfile")]
public class VoiceProfile : ScriptableObject
{
    public AudioClip[] voiceLines;
    public AudioClip deathVoice;

    public AudioClip GetRandomVoiceLine()
    {
        return voiceLines[Random.Range(0, voiceLines.Length)];
    }
}

