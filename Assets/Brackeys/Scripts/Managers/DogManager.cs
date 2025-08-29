using UnityEngine;
using UnityEngine.AI;
public class DogManager : MonoBehaviour
{
    public DogScriptableBehaviour dogData;
    private GameObject dogInstance;
    private NavMeshAgent agent;

    public float currentHealth;
    private Transform targetWaypoint;
    private int waypointIndex = 0;
    public float currentDogSpeed = 0;

    void Start()
    {
        currentHealth = dogData.dogMaxHealth;
        targetWaypoint = Waypoints.points[Waypoints.points.Length - 1];

        Vector3 spawnPos = new Vector3(transform.position.x, dogData.dogPrefab.transform.position.y, transform.position.z);
        //TODO use rotation of spawn point
        dogInstance = Instantiate(dogData.dogPrefab, spawnPos, dogData.dogPrefab.transform.rotation, transform);
        currentDogSpeed = dogData.dogSpeed;
        agent = dogInstance.GetComponent<NavMeshAgent>();
        agent.SetDestination(targetWaypoint.position);
        agent.speed = currentDogSpeed;
    }

    void Update()
    {
        if ((agent.remainingDistance) < 0.1f)
        {
            {
                //TODO reach end
                Destroy(gameObject);
                return;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            RunAway();
        }
    }
    public void ModifySpeed(float amount)
    {
        currentDogSpeed = amount;
        agent.speed = currentDogSpeed;
    }
    public void ResetSpeed()
    {
        currentDogSpeed = dogData.dogSpeed;
        agent.speed = currentDogSpeed;
    }

    void RunAway()
    {
        TowerManager[] towers = FindObjectsOfType<TowerManager>();
        foreach (var tower in towers)
        {
            tower.RemoveDog(transform);
        }

        transform.tag = "Untagged";
        agent.tag = "Untagged";
        //  Destroy(gameObject);
        agent.SetDestination(transform.position);
    }
}