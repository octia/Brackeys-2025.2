using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TDPath : MonoBehaviour
{
    [Tooltip("Waypoints in order that dogs will follow.")]
    public List<Waypoint> waypoints = new List<Waypoint>();

    [Range(0f, 1f)] public float gizmoT = 0f; 
    public Color lineColor = new Color(1f, 0.6f, 0.1f, 1f);

    public int Count => waypoints.Count;

    public Vector3 GetPoint(int index) => waypoints[Mathf.Clamp(index, 0, Count - 1)].transform.position;

    public Vector3 Sample(float t)
    {
        if (Count == 0) return transform.position;
        if (Count == 1) return GetPoint(0);

        float scaled = t * (Count - 1);
        int a = Mathf.FloorToInt(scaled);
        int b = Mathf.Min(a + 1, Count - 1);
        float lt = Mathf.Clamp01(scaled - a);
        return Vector3.Lerp(GetPoint(a), GetPoint(b), lt);
    }

    private void OnDrawGizmos()
    {
        if (Count < 2) return;
        Gizmos.color = lineColor;

        for (int i = 0; i < Count - 1; i++)
            Gizmos.DrawLine(GetPoint(i), GetPoint(i + 1));

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(Sample(gizmoT), 0.15f);
    }
}
