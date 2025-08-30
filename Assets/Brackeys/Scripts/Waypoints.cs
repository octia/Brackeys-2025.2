using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    // public static Transform[] points;

    public static List<Transform[]> pointsList;
    void Awake()
    {
        pointsList = new List<Transform[]>();
        for (int k = 0; k < transform.childCount; k++)
        {
            var points = new Transform[transform.GetChild(k).childCount];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.GetChild(k).GetChild(i);
            }
            pointsList.Add(points);
        }

    }
}
