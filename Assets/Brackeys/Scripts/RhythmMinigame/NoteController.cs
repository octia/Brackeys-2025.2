using UnityEngine;
using UnityEngine.InputSystem;

public class NoteController : MonoBehaviour
{
    public RhythmController rhythmController;

    [Space]
    [SerializeField] private string performInputName;

    // ...
    private InputAction performInput;

    private void Start()
    {
        performInput = InputSystem.actions.FindAction(performInputName);
    }

    private void Update()
    {
        // Note movement
        transform.position += new Vector3(0, -1) * Time.deltaTime * rhythmController.speed;

        // Failed note
        if (transform.position.y < rhythmController.failPoint.position.y)
        {
            Destroy(gameObject);

            // TODO: Add fail animation
            Debug.Log("Note failed");
        }

        // Perform note
        if (performInput.WasPressedThisFrame() && transform.position.y < rhythmController.trackPerformPoint.position.y)
        {
            rhythmController.biscuitManager.Biscuit++;
            Destroy(gameObject);

            // TODO: Add perform animation
            Debug.Log("Note performed");
        }
    }
}