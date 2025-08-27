using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    // ...
    private InputAction movementAction;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 movementInput = movementAction.ReadValue<Vector2>().normalized;

        rb.linearVelocity = new Vector3(movementInput.x, 0, movementInput.y) * speed;
    }
}
