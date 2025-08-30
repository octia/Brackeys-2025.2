using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class BakeryController : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    [SerializeField] private GameObject produceButton;

    [Space]
    [SerializeField] private RhythmController rhythmController;

    [Inject]
    private PlayerController playerController;

    [Inject]
    private TimerManager timerManager;

    private InputAction interactInput;

    private void Start()
    {
        interactInput = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        produceButton.gameObject.SetActive(Vector3.Distance(playerController.player.position, transform.position) < interactionRange && !timerManager.IsPaused);

        if (produceButton.activeSelf && interactInput.WasPressedThisFrame())
        {
            rhythmController.ProduceButton();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
