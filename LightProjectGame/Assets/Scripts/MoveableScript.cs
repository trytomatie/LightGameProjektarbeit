using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableScript : MonoBehaviour
{
    public Vector3 movement;
    public Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        lastPosition = transform.position;
    }

    public Vector3 CalculatePosition()
    {
        movement = transform.position - lastPosition;
        lastPosition = transform.position;
        return movement;
    }
}
