using System.Collections;
using System.Collections.Generic;
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
    public float resolution = 10f;
    public float threshold = 0.5f;
    public float pauseTime = 2f;
    public float offsetScale = 5f;
    public int damage = 5;
    public int runAwaySpeed = 10;

    int direction = 1;
    Vector3[] splinePoints;
    bool isPausedForAnimation;

    private CapsuleCollider myAgentsCollider;
    private Rigidbody myAgentsRigidbody;

    private TimerManager timerManager;
    private BiscuitManager biscuitManager;

    private Vector3 lastPos;
    public Vector3 currentVelocity;


    void Start()
    {

        currentHealth = dogData.dogMaxHealth;
        targetWaypoint = Waypoints.points[0];

        Vector3 spawnPos = new Vector3(transform.position.x, dogData.dogPrefab.transform.position.y, transform.position.z);
        //TODO use rotation of spawn point
        dogInstance = Instantiate(dogData.dogPrefab, spawnPos, dogData.dogPrefab.transform.rotation, transform);
        currentDogSpeed = dogData.dogSpeed;

        timerManager = transform.parent.GetComponent<WaveSpawner>().timerManager;
        biscuitManager = transform.parent.GetComponent<WaveSpawner>().biscuitManager;
        agent = dogInstance.GetComponent<NavMeshAgent>();
        agent.SetDestination(targetWaypoint.position);
        agent.speed = currentDogSpeed;

        splinePoints = GetSplinePoints(Waypoints.points, resolution);

        myAgentsCollider = agent.GetComponent<CapsuleCollider>();
        myAgentsRigidbody = agent.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (timerManager.IsPaused == true)
        {
            agent.isStopped = true;
            myAgentsRigidbody.isKinematic = true;
        }
        else
        {
            myAgentsRigidbody.isKinematic = false;
            agent.isStopped = false;
        }

        if (isPausedForAnimation == true || splinePoints.Length == 0) return;
        if (!agent.pathPending && agent.remainingDistance < threshold)
        {
            waypointIndex += direction;
            if (waypointIndex >= splinePoints.Length || waypointIndex < 0)
            {
                Destroy(gameObject);

                // TODO: Change the amount of damage to be based on bakery upgrade
                biscuitManager.Biscuit -= damage;

                return;
            }
            agent.SetDestination(splinePoints[waypointIndex]);
        }

        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            currentVelocity = (child.position - lastPos) / Time.deltaTime;
            lastPos = child.position;
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
        StartCoroutine(PauseAndReverse());
    }



    IEnumerator PauseAndReverse()
    {
        isPausedForAnimation = true;
        agent.ResetPath();
        agent.isStopped = true;
        yield return new WaitForSeconds(pauseTime);
        ModifySpeed(runAwaySpeed);
        myAgentsCollider.radius = 0.1f;
        myAgentsCollider.height = 0.1f;
        direction *= -1;
        waypointIndex += direction;
        waypointIndex += direction;
        waypointIndex = Mathf.Max(0, waypointIndex);
        waypointIndex = 0;
        agent.isStopped = false;
        agent.SetDestination(splinePoints[waypointIndex]);
        isPausedForAnimation = false;
    }

    Vector3[] GetSplinePoints(Transform[] cps, float res)
    {
        if (cps.Length < 2) return new Vector3[0];
        List<Vector3> points = new List<Vector3>();


        Vector3[] padded = new Vector3[cps.Length + 2];
        padded[0] = cps[0].position;
        for (int i = 0; i < cps.Length; i++)
        {
            //Adding small offset for each dog spline
            Vector3 randomXZ = Random.insideUnitSphere;
            randomXZ.y = 0f;
            randomXZ.Normalize();
            padded[i + 1] = cps[i].position + randomXZ / offsetScale;
        }
        padded[padded.Length - 1] = cps[cps.Length - 1].position;


        for (int i = 0; i < padded.Length - 3; i++)
        {
            for (int j = 0; j < res; j++)
            {
                float t = j / res;
                Vector3 p = CatmullRom(
                    padded[i], padded[i + 1], padded[i + 2], padded[i + 3], t);
                points.Add(p);
            }
        }

        points.Add(padded[padded.Length - 2]);
        return points.ToArray();
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < splinePoints.Length - 1; i++)
            Gizmos.DrawLine(splinePoints[i], splinePoints[i + 1]);
    }
}




