using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float radius = 1f;
    public Vector2 regionSize = new Vector2(10,10);
    public int rejectionSamples = 30;
    public float displayRadius = 0.2f;

    List<Vector2> points;


    void OnValidate()
    {
        if(radius <= 0)
            return;

        points = PoissonDiscSampling.GeneratePoints(
            radius,
            regionSize,
            rejectionSamples
        );
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(
            regionSize / 2,
            regionSize
        );

        if(points == null)
            return;

        foreach(Vector2 point in points)
        {
            Gizmos.DrawSphere(
                new Vector3(point.x,0,point.y),
                displayRadius
            );
        }
    }
}