using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    [Space]
    [SerializeField] private GameObject buildButton;
    [SerializeField] private TowersPanel towersPanel;

    private TowerManager towerManager;

    private InputAction interactAction;

    [Inject]
    private PlayerController playerController;

    [Inject]
    private TimerManager timerManager;

    private void Start()
    {
        towerManager = GetComponent<TowerManager>();

        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        buildButton.SetActive(Vector3.Distance(playerController.player.position, transform.position) < interactionRange && !timerManager.IsPaused);

        if (buildButton.activeSelf && interactAction.WasPressedThisFrame())
        {
            BuildButton();
        }
    }

    public void BuildButton()
    {
        towersPanel.ShowButton(towerManager);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
