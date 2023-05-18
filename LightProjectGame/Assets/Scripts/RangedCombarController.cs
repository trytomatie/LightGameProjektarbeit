using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombarController : State
{
    private PlayerController playerController;
    private LockOnState lockOnState;
    private Camera mainCamera;
    public GameObject projectile;
    public Vector3 shootOffset = new Vector3(0,0.6f,0);
    public LayerMask layerMask;
    public GameObject rcTarget;
    public GameObject reticle;
    public GameObject rcTargetHighlightPrefab;
    public GameObject draggableHighlightPrefab;

    private GameObject projectedHighlight;


    private GameObject[] draggableObjects;
    private List<GameObject> highlightObjectIndicators = new List<GameObject>();
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mainCamera = Camera.main;
        lockOnState = GetComponent<LockOnState>();
    }

    void Rotation()
    {
        float rotation = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), 360 * Time.deltaTime);
    }

    void HandleShooting()
    {
        if(Input.GetMouseButtonDown(0) && playerController.myStatus.LightEnergy>= 5)
        {
            playerController.myStatus.LightEnergy -= 5;
            RaycastHit raycastHit;

            //if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 50, layerMask))
            //{
            //    Vector3 direction = (transform.position + shootOffset) - raycastHit.point;
            //    GameObject _projectile = Instantiate(projectile, transform.position + shootOffset, Quaternion.identity);
            //    _projectile.transform.LookAt(raycastHit.point);
            //}
            //else
            //{
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            GameObject _projectile = Instantiate(projectile, transform.position + shootOffset, Quaternion.identity); 
            _projectile.transform.LookAt(ray.GetPoint(15));
            //}
        }
    }

    private void RaycastForObjects()
    {
        RaycastHit rc;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward,out rc, 7, layerMask))
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
        HandleShooting();
        playerController.HandleLantern();
        RaycastForObjects();
        lockOnState.AnimationsParemetersInput();
    }

    public override StateName Transition(GameObject source)
    {
        if (!Input.GetMouseButton(1))
        {
            return StateName.Controlling;
        }
        if(Input.GetKeyDown(KeyCode.E) && rcTarget != null)
        {
            return StateName.Dragging;
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
    }
    #endregion
}
