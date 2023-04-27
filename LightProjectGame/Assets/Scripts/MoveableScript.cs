using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableScript : MonoBehaviour
{
    public Vector3 movement;
    public Vector3 lastPosition;
    public float lastScale;
    // Start is called before the first frame update
    void Start()
    {
        movement = Vector3.zero;
        lastPosition = transform.position;
        lastScale = transform.localScale.y;
    }

    private void Update()
    {
        movement = (transform.position - lastPosition) + ((transform.localScale.y - lastScale) * transform.up);
        lastPosition = transform.position;
        lastScale = transform.localScale.y;
    }
    public Vector3 CalculatePosition()
    {
        return movement;
    }
}
