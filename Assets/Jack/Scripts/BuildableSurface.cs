using System.Collections.Generic;
using UnityEngine;

public class BuildableSurface : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask buildableLayer;
    public LayerMask noBuildLayer;

    [Header("Rules")]
    public float minDistFromPath = 1.0f;
    public float minDistFromOtherTowers = 1.0f;
    public float groundCheckMaxDistance = 200f;

    [Header("Scene References")]
    public List<TDPath> enemyPaths = new();

    public bool TryGetBuildPoint(Ray ray, out Vector3 point)
    {
        point = default;

        if (Physics.Raycast(ray, out var hit, groundCheckMaxDistance, buildableLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 p = hit.point;

            if (Physics.CheckSphere(p, 0.1f, noBuildLayer, QueryTriggerInteraction.Collide))
                return false;

            foreach (var path in enemyPaths)
            {
                if (!path || path.Count < 2) continue;
                if (DistanceToPath(p, path) < minDistFromPath)
                    return false;
            }

            foreach (var t in GameObject.FindGameObjectsWithTag("Tower"))
            {
                if ((t.transform.position - p).sqrMagnitude < minDistFromOtherTowers * minDistFromOtherTowers)
                    return false;
            }

            point = p;
            return true;
        }
        return false;
    }

    float DistanceToPath(Vector3 p, TDPath path)
    {
        const int steps = 32;
        float min = float.MaxValue;
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            float d = Vector3.Distance(p, path.Sample(t));
            if (d < min) min = d;
        }
        return min;
    }
}
