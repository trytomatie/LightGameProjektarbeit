using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForDragables : State
{
    private PlayerController playerController;
    private LockOnState lockOnState;
    private Camera mainCamera;
    public LayerMask layerMask;
    public GameObject rcTarget;
    public GameObject reticle;
    public GameObject rcTargetHighlightPrefab;
    public GameObject draggableHighlightPrefab;
    public Transform ikTarget;

    private GameObject projectedHighlight;


    private GameObject[] draggableObjects;
    private List<GameObject> highlightObjectIndicators = new List<GameObject>();

    private StateMachine sm;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mainCamera = Camera.main;
        lockOnState = GetComponent<LockOnState>();
        sm = GetComponent<StateMachine>();
    }

    void Rotation()
    {
        float rotation = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 360 * Time.deltaTime);
    }

    private void RaycastForObjects()
    {
        RaycastHit rc;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward,out rc, 12, layerMask))
        {
            if(rc.collider.tag == "Dragable")
            {
                if(rcTarget != rc.collider.gameObject)
                {
                    if (projectedHighlight != null)
                    {
                        Destroy(projectedHighlight);
                    }
                    rcTarget = rc.collider.gameObject;
                    projectedHighlight = Instantiate(rcTargetHighlightPrefab, rcTarget.transform.position, rcTarget.transform.rotation, rcTarget.transform);
                    projectedHighlight.GetComponent<MeshFilter>().mesh = rcTarget.GetComponent<MeshFilter>().mesh;
                    projectedHighlight.transform.localPosition = projectedHighlight.transform.localPosition - new Vector3(0, -0.001f, 0);
                    projectedHighlight.transform.localScale = new Vector3(1.02f, 1.02f, 1.02f);
                }
                return;
            }
        }
        rcTarget = null;
        if (rcTarget == null)
        {
            Destroy(projectedHighlight);
        }
    }

    private void SetIKTargetPosition()
    {
        ikTarget.position = mainCamera.transform.position + mainCamera.transform.forward * 10;
    }

    #region StateMethods
    public override void EnterState(GameObject source)
    {
        playerController.camAnim.SetInteger("cam", 1);
        reticle.SetActive(true);
        draggableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        foreach(GameObject go in draggableObjects)
        {
            GameObject highLight = Instantiate(draggableHighlightPrefab, go.transform.position, go.transform.rotation, go.transform);
            highLight.GetComponent<MeshFilter>().mesh = go.GetComponent<MeshFilter>().mesh;
            highLight.transform.localScale = new Vector3(1.01f, 1.01f, 1.01f);
            highLight.transform.localPosition = highLight.transform.localPosition - new Vector3(0, -0.001f, 0);
            highlightObjectIndicators.Add(highLight);
        }
        playerController.anim.SetFloat("movementMode", 1);
    }

    public override void UpdateState(GameObject source)
    {
        playerController.Movement();
        Rotation();
        playerController.Animations();
        playerController.CalculateGravity();
        playerController.HandleLantern();
        RaycastForObjects();
        lockOnState.AnimationsParemetersInput();
        SetIKTargetPosition();
    }

    public override StateName Transition(GameObject source)
    {
        if (!Input.GetMouseButton(1))
        {
            return StateName.Controlling;
        }
        if(Input.GetMouseButtonDown(0) && rcTarget != null)
        {
            return StateName.Dragging;
        }
        if (playerController.selectedSkill == 1)
        {
            return StateName.Aiming;
        }
        return stateName;
    }

    public override void ExitState(GameObject source)
    {
        if(projectedHighlight != null)
        {
            Destroy(projectedHighlight);
        }
        playerController.camAnim.SetInteger("cam", 0);
        reticle.SetActive(false);
        for(int i = 0; i < highlightObjectIndicators.Count;i++)
        {
            Destroy(highlightObjectIndicators[i]);
        }
        highlightObjectIndicators.Clear();
        playerController.anim.SetFloat("movementMode", 0);
        playerController.anim.SetBool("aiming", false);
    }
    #endregion
}
