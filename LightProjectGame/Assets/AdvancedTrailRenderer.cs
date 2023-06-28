using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SplineComputer))]
public class AdvancedTrailRenderer : MonoBehaviour
{
    private SplineComputer spline;
    public float size = 0.3f;
    private List<SplinePoint> points = new List<SplinePoint>();
    public Transform target;
    private bool trailActive = false;
    private Vector3 lastTrailPos;



    // Start is called before the first frame update
    void Start()
    {
        spline = GetComponent<SplineComputer>();
        points.Add(new SplinePoint());
    }

    // Update is called once per frame
    void Update()
    {
        if (TrailActive)
        {
            lastTrailPos = target.position;

        }
        else if (points.Last().position == lastTrailPos)
        {
            return;
        }

        SplinePoint sp = new SplinePoint(lastTrailPos);
        sp.size = size;
        points.Insert(0, sp);
        if(points.Count > 10)
        {
            points.RemoveAt(points.Count-1);
        }
        spline.SetPoints(points.ToArray());
    }

    public bool TrailActive
    {
        get => trailActive; set
        {
            trailActive = value;
            if(trailActive)
            {
                points = new List<SplinePoint>();

            }
        }
    }
}
