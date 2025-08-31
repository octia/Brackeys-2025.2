using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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
    public float threshold = 0.05f;
    public float pauseTime = 2f;
    public float offsetScale = 5f;
    public int runAwaySpeed = 10;

    public int waypointLineIndex = 0;
    int direction = 1;
    Vector3[] splinePoints;
    bool isPausedForAnimation;

    private CapsuleCollider myAgentsCollider;
    private Rigidbody myAgentsRigidbody;

    public TimerManager timerManager;
    public BiscuitManager biscuitManager;
    public RhythmController rhythmController;

    private Vector3 lastPos;
    public Vector3 currentVelocity;

    private Animator anim;

    private Transform exit;
    private bool isRunningAway = false;

    private float attentionBarWidth;

    void Start()
    {
        currentHealth = dogData.dogMaxHealth;
        targetWaypoint = Waypoints.pointsList[waypointLineIndex][0];

        Vector3 spawnPos = new Vector3(transform.position.x, dogData.dogPrefab.transform.position.y, transform.position.z);
        //TODO use rotation of spawn point
        dogInstance = Instantiate(dogData.dogPrefab, spawnPos, dogData.dogPrefab.transform.rotation, transform);
        currentDogSpeed = dogData.dogSpeed;

        //timerManager = transform.parent.GetComponent<WaveSpawner>().timerManager;
        //biscuitManager = transform.parent.GetComponent<WaveSpawner>().biscuitManager;
        agent = dogInstance.GetComponent<NavMeshAgent>();
        agent.SetDestination(targetWaypoint.position);
        agent.speed = currentDogSpeed;

        splinePoints = GetSplinePoints(Waypoints.pointsList[waypointLineIndex], resolution);

        myAgentsCollider = agent.GetComponent<CapsuleCollider>();
        myAgentsRigidbody = agent.GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();
        exit = GameObject.FindGameObjectWithTag("Exit").transform;

        attentionBarWidth = dogInstance.GetComponentsInChildren<RawImage>(true)[1].rectTransform.sizeDelta.x;
    }

    void Update()
    {
        if (timerManager.IsPaused)
        {
            anim.speed = 0;

            agent.isStopped = true;
            myAgentsRigidbody.isKinematic = true;
        }
        else
        {
            anim.speed = 1;

            myAgentsRigidbody.isKinematic = false;
            agent.isStopped = false;
        }

        if (isPausedForAnimation == true || splinePoints.Length == 0) return;


        if (!agent.pathPending && agent.remainingDistance < threshold)
        {
            if (isRunningAway == true)
            {
                Destroy(gameObject);
            }
            else
            {
                waypointIndex += direction;
                if (waypointIndex >= splinePoints.Length || waypointIndex < 0)
                {
                    RunAway();
                    //reach to end and do stuff about that
                    // TODO: Change the amount of damage to be based on bakery upgrade
                    biscuitManager.Biscuit -= Mathf.FloorToInt(dogData.dogDamage);

                    if (rhythmController.main.activeSelf)
                    {
                        rhythmController.ProduceButton();
                    }

                    return;
                }
                agent.SetDestination(splinePoints[waypointIndex]);
            }

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

        RectTransform rectTransform = dogInstance.GetComponentsInChildren<RawImage>(true)[1].rectTransform;

        currentHealth -= amount;
        if (currentHealth != dogData.dogMaxHealth)
        {
            dogInstance.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
            rectTransform.sizeDelta = new Vector2(attentionBarWidth - attentionBarWidth / dogData.dogMaxHealth * currentHealth, rectTransform.sizeDelta.y);
        }
        else
        {
            dogInstance.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
        }

        if (currentHealth <= 0)
        {
            rectTransform.sizeDelta = new Vector2(attentionBarWidth, rectTransform.sizeDelta.y);
            RunAway();
        }
    }
    public void ModifySpeed(float amount)
    {
        if (isRunningAway) return;

        currentDogSpeed = amount;
        agent.speed = currentDogSpeed;
    }
    public void ResetSpeed()
    {
        if (isRunningAway) return;

        currentDogSpeed = dogData.dogSpeed;
        agent.speed = currentDogSpeed;
    }

    public void LookAtTarget(Vector3 target)
    {
        agent.transform.rotation = Quaternion.LookRotation(target - agent.transform.position, Vector3.up);
        anim.SetTrigger("Distract");
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
        isRunningAway = true;
        myAgentsCollider.radius = 0.1f;
        myAgentsCollider.height = 0.1f;
        direction *= -1;
        waypointIndex = 0;
        agent.isStopped = false;
        agent.SetDestination(exit.position);
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




