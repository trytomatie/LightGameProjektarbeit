using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVoiceLineManager : MonoBehaviour
{

    public VoiceProfile[] profilesSmall;
    public VoiceProfile[] profilesMid;

    public static EnemyVoiceLineManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
