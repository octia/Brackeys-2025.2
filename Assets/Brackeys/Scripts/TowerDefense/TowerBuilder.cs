using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    [SerializeField] private int buildCost;

    [Space]
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material freeMaterial;
    [SerializeField] private Material buyableMaterial;
    [SerializeField] private TMP_Text costText;

    [Space]
    [SerializeField] private PopupAnimator buildButton;
    [SerializeField] private TowersPanel towersPanel;

    private TowerManager towerManager;

    private InputAction interactAction;

    [Inject]
    private PlayerController playerController;

    [Inject]
    private TimerManager timerManager;

    [Inject]
    private BiscuitManager biscuitManager;

    private void Start()
    {
        towerManager = GetComponent<TowerManager>();

        interactAction = InputSystem.actions.FindAction("Interact");

        CostUpdate();

    }

    private void Update()
    {
        buildButton.SetActive(Vector3.Distance(playerController.player.position, transform.position) < interactionRange && !timerManager.IsPaused);

        if (buildButton.enabled && interactAction.WasPressedThisFrame())
        {
            BuildButton();
        }
    }

    public void BuildButton()
    {
        if (biscuitManager.Biscuit - buildCost < 1)
        {
            return;
        }
        else
        {
            biscuitManager.Biscuit -= buildCost;
            buildCost = 0;
            CostUpdate();
        }

        towersPanel.ShowButton(towerManager);
    }

    private void CostUpdate()
    {
        mesh.material = buildCost == 0 ? freeMaterial : buyableMaterial;
        costText.transform.parent.gameObject.SetActive(buildCost != 0);
        costText.text = buildCost + " biscuits for this spot!";
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
