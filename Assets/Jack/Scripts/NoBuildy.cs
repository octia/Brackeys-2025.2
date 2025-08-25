using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider))]
public class NoBuildy : MonoBehaviour
{
    public Color gizmoColor = new Color(1f, 0f, 0f, 0.2f);

    private void OnDrawGizmos()
    {
        var bc = GetComponent<BoxCollider>();
        if (!bc) return;

        Gizmos.color = gizmoColor;
        var m = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = m;
        Gizmos.DrawCube(bc.center, bc.size);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
