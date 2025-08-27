using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DogManager : MonoBehaviour
{
    public DogScriptableBehaviour dogData;
    private GameObject dogInstance;

    public float currentHealth;
    private Transform targetWaypoint;
    private int waypointIndex = 0;
    public float currentDogSpeed = 0;

    void Start()
    {
        currentHealth = dogData.dogMaxHealth;
        targetWaypoint = Waypoints.points[0];

        Vector3 spawnPos = new Vector3(transform.position.x, dogData.dogPrefab.transform.position.y, transform.position.z);
        dogInstance = Instantiate(dogData.dogPrefab, spawnPos, Quaternion.identity, transform);
        currentDogSpeed = dogData.dogSpeed;
    }

    void Update()
    {
        Vector3 dir = targetWaypoint.position - transform.position;
        transform.Translate(dir.normalized * currentDogSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            waypointIndex++;
            if (waypointIndex >= Waypoints.points.Length)
            {
                Destroy(gameObject);
                return;
            }
            targetWaypoint = Waypoints.points[waypointIndex];
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            RunAway();
        }
    }
    public void ModifySpeed(float amount)
    {
        currentDogSpeed = amount;
    }
    public void ResetSpeed()
    {
        currentDogSpeed = dogData.dogSpeed;
    }

    void RunAway()
    {
        TowerManager[] towers = FindObjectsOfType<TowerManager>();
        foreach (var tower in towers)
        {
            tower.RemoveDog(transform);
        }

        Destroy(gameObject);
    }
}