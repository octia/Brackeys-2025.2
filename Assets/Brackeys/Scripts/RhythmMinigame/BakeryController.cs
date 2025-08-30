using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class BakeryController : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    public MaxBiscuitLevel[] maxBiscuitLevels;
    public BiscuitMultiplierLevel[] biscuitMultiplierLevels;

    [Space]
    [SerializeField] private GameObject produceButton;

    [Space]
    [SerializeField] private RhythmController rhythmController;

    [Inject]
    private PlayerController playerController;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private BiscuitManager biscuitManager;

    private InputAction interactInput;

    // ...
    [HideInInspector] public int currentMaxBiscuitLevel;
    [HideInInspector] public int currentBiscuitMultiplierLevel;

    private void Start()
    {
        interactInput = InputSystem.actions.FindAction("Interact");
        
        rhythmController.baseMultiplier = biscuitMultiplierLevels[currentBiscuitMultiplierLevel].multiplier;
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

    public void UpgradeMaxBiscuitLevel()
    {
        currentMaxBiscuitLevel += 1;
        biscuitManager.maxBiscuits = maxBiscuitLevels[currentMaxBiscuitLevel].maxBiscuit;
    }

    public void UpgradeMultiplierLevel()
    {
        currentBiscuitMultiplierLevel += 1;
        rhythmController.baseMultiplier = biscuitMultiplierLevels[currentBiscuitMultiplierLevel].multiplier;
    }
}

[System.Serializable]
public class MaxBiscuitLevel
{
    public int cost;
    public int maxBiscuit;
}

[System.Serializable]
public class BiscuitMultiplierLevel
{
    public int cost;
    public int multiplier;
}