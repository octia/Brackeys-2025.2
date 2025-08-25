using UnityEngine;

[ExecuteAlways]
public class Waypoint : MonoBehaviour
{
    [Min(0f)] public float waitSeconds = 0f;
    [Min(0f)] public float radius = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
