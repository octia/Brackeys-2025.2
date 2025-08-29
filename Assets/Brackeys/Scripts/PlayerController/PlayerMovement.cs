using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    // ...
    private InputAction movementAction;
    private InputAction runAction;
    private Rigidbody rb;

    // ...
    [HideInInspector] public bool canMove = true;

    [Inject]
    private TimerManager timerManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        movementAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        if (!canMove || timerManager.IsPaused)
        {
            return;
        }

        Vector2 movementInput = movementAction.ReadValue<Vector2>().normalized;

        float speed = runAction.IsPressed() ? runSpeed : walkSpeed;

        rb.linearVelocity = new Vector3(movementInput.x, 0, movementInput.y) * speed;
    }
}
