using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    [Space]
    [SerializeField] private GameObject buildButton;
    [SerializeField] private TowersPanel towersPanel;

    [Inject]
    private PlayerController playerController;

    [Inject]
    private TimerManager timerManager;

    private TowerManager towerManager;

    private void Start()
    {
        towerManager = GetComponent<TowerManager>();
    }

    private void Update()
    {
        buildButton.SetActive(Vector3.Distance(playerController.player.position, transform.position) < interactionRange && !timerManager.IsPaused);
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
