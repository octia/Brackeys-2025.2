using UnityEngine;
using UnityEngine.InputSystem;

public class TowerHover : MonoBehaviour
{
    public TowerManager parent;
    private bool isHovering = false;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
        {
            if (!isHovering)
            {
                isHovering = true;
                parent.ShowRange(true);
            }
        }
        else
        {
            if (isHovering)
            {
                isHovering = false;
                parent.ShowRange(false);
            }
        }
    }

}
