using Reflex.Attributes;
using UnityEngine;

public class BakeryController : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    [SerializeField] private GameObject produceButton;

    [Inject]
    PlayerController playerController;

    private void Update()
    {
        produceButton.gameObject.SetActive(Vector3.Distance(playerController.player.position, transform.position) < interactionRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
