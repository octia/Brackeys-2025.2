using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    // ...
    private InputAction movementAction;
    private Rigidbody rb;

    // ...
    [HideInInspector] public bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        Vector2 movementInput = movementAction.ReadValue<Vector2>().normalized;

        rb.linearVelocity = new Vector3(movementInput.x, 0, movementInput.y) * speed;
    }
}
