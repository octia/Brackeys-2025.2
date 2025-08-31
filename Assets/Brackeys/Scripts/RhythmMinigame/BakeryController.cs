using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class BakeryController : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    public MaxBiscuitLevel[] maxBiscuitLevels;
    public BiscuitMultiplierLevel[] biscuitMultiplierLevels;

    [Space]
    [SerializeField] private PopupAnimator produceButton;
    [SerializeField] public GameObject infoButton;
    [SerializeField] private EventReference buttonClickSfx;

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
        produceButton.SetActive(Vector3.Distance(playerController.player.position, transform.position) < interactionRange && !timerManager.IsPaused);

        if (produceButton.enabled && interactInput.WasPressedThisFrame())
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
        PlayButtonSfx();
    }

    public void UpgradeMultiplierLevel()
    {
        currentBiscuitMultiplierLevel += 1;
        rhythmController.baseMultiplier = biscuitMultiplierLevels[currentBiscuitMultiplierLevel].multiplier;
        PlayButtonSfx();
    }

    private void PlayButtonSfx(EventReference? sfxOverride = null)
    {
        EventReference sfxToPlay = sfxOverride.HasValue ? sfxOverride.Value : buttonClickSfx;

        if (!sfxToPlay.IsNull)
            RuntimeManager.PlayOneShot(sfxToPlay);
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