using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager instance;
    public MMF_Player footStep_Feedback;
    public MMF_Player footStep_Feedback_Stone;
    public MMF_Player weaponHit_Feedback;
    public MMF_Player weaponWiff_Feedback;
    public MMF_Player magnet_Feedback;
    public MMF_Player shieldBreak_Feedback;
    public MMF_Player readyShield_Feedback;
    public MMF_Player successfulBlock_Feedback;
    public MMF_Player shootCharge_Feedback;
    public MMF_Player shootCharged_Feedback;
    public MMF_Player shoot_Feedback;



    public MMF_Player pickUPItem_Feedback;

    public MMF_Player voiceLinesHolder_Feedback;

    public MMF_Player enemyVoiceLinesHolder_Feedback;
    public MMF_Player vfxHolder_Feedback;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySound(MMF_Player sound, Transform position)
    {
        MMF_MMSoundManagerSound soundSource = (MMF_MMSoundManagerSound)sound.FeedbacksList[0];
        soundSource.AttachToTransform = position;
        sound.PlayFeedbacks();
    }

    public static void PlayVoiceLine(AudioClip soundFile)
    {
        MMF_MMSoundManagerSound soundSource = (MMF_MMSoundManagerSound)instance.voiceLinesHolder_Feedback.FeedbacksList[0];
        soundSource.Sfx = soundFile;
        instance.voiceLinesHolder_Feedback.PlayFeedbacks();
    }


    public static void PlayVoiceEnemyLine(AudioClip soundFile,Transform position)
    {
        MMF_MMSoundManagerSound soundSource = (MMF_MMSoundManagerSound)instance.enemyVoiceLinesHolder_Feedback.FeedbacksList[0];
        soundSource.Sfx = soundFile;
        soundSource.AttachToTransform = position;
        instance.enemyVoiceLinesHolder_Feedback.PlayFeedbacks();
    }

    public static void PlaySFX(AudioClip soundFile)
    {
        MMF_MMSoundManagerSound soundSource = (MMF_MMSoundManagerSound)instance.vfxHolder_Feedback.FeedbacksList[0];
        soundSource.Sfx = soundFile;
        instance.vfxHolder_Feedback.PlayFeedbacks();
    }

    public static void PlaySFX(AudioClip soundFile, Transform position)
    {
        MMF_MMSoundManagerSound soundSource = (MMF_MMSoundManagerSound)instance.vfxHolder_Feedback.FeedbacksList[0];
        soundSource.Sfx = soundFile;
        soundSource.AttachToTransform = position;
        instance.vfxHolder_Feedback.PlayFeedbacks();
    }

}
