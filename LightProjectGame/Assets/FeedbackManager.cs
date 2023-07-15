using MoreMountains.Feedbacks;
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

}
