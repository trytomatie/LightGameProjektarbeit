using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectHandler : MonoBehaviour
{
    public LayerMask layerMask;
    private CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        // Checks if moveableObject it benath player
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f,0), new Vector3(0, -1, 0), out raycastHit, 4f, layerMask))
        {
            // add velocity of moveable Object
            if(raycastHit.collider.GetComponent<MoveableScript>() != null)
            {
                cc.Move(raycastHit.collider.GetComponent<MoveableScript>().CalculatePosition());
            }
        }
    }
}
