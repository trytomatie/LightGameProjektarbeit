using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect[] vfx;
    public Animator[] slashvfx;
    public GameObject[] hitboxes;
    private Animator anim;
    public List<Material> stoneMaterial = new List<Material>();

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
    }

    public void TriggerVFX(int i)
    {
        VisualEffect _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation,vfx[i].transform.parent);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.parent = null;
        Destroy(_vfx, 10f);
        //vfx[i].Play();
    }

    public void TriggerSlashVFX(int i)
    {
        GameObject vfx = slashvfx[i].gameObject;
        GameObject _vfx = Instantiate(vfx, vfx.transform.position, vfx.transform.rotation, vfx.transform.parent);
        _vfx.SetActive(true);
        _vfx.transform.localScale = vfx.transform.localScale;
        _vfx.transform.parent = null;
        Destroy(_vfx, 10f);
    }
    public void TriggerVFXSpam(int i)
    {
        VisualEffect _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        _vfx = Instantiate(vfx[i], vfx[i].transform.position, vfx[i].transform.rotation);
        _vfx.transform.localScale = vfx[i].transform.localScale;
        _vfx.transform.eulerAngles = Random.insideUnitSphere * 360;
        Destroy(_vfx, 1f);
        //vfx[i].Play();
    }

    public void TriggerHitbox(AnimationEvent myEvent)
    {
        hitboxes[myEvent.intParameter].SetActive(true);
        StartCoroutine(DisableHitbox(myEvent.intParameter, myEvent.floatParameter));

    }

    IEnumerator DisableHitbox(int i,float duration)
    {
        yield return new WaitForSeconds(duration);
        hitboxes[i].SetActive(false);
    }

    public void PlayFootstepSound()
    {
        if(anim.GetCurrentAnimatorStateInfo(2).IsName("Empty"))
        {
            RaycastHit rc;
            Physics.Raycast(transform.position + new Vector3(0,0.6f,0), new Vector3(0, -1, 0), out rc, 2);
            print(rc.collider.gameObject);
            if(rc.collider.GetComponent<Renderer>() != null)
            {
                Material mat = rc.collider.GetComponent<Renderer>().sharedMaterial;
                if (stoneMaterial.Contains(mat)) 
                {
                    MMF_MMSoundManagerSound sound = (MMF_MMSoundManagerSound)FeedbackManager.instance.footStep_Feedback_Stone.FeedbacksList[0];
                    sound.AttachToTransform = transform;
                    FeedbackManager.instance.footStep_Feedback_Stone.PlayFeedbacks();
                }
                else
                {
                    MMF_MMSoundManagerSound sound = (MMF_MMSoundManagerSound)FeedbackManager.instance.footStep_Feedback.FeedbacksList[0];
                    sound.AttachToTransform = transform;
                    FeedbackManager.instance.footStep_Feedback.PlayFeedbacks();
                }
            }
            else
            {
                MMF_MMSoundManagerSound sound = (MMF_MMSoundManagerSound)FeedbackManager.instance.footStep_Feedback.FeedbacksList[0];
                sound.AttachToTransform = transform;
                FeedbackManager.instance.footStep_Feedback.PlayFeedbacks();
            }



        }
    }

}
