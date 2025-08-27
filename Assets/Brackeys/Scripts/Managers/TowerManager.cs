using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public TowerScriptableObject towerData;
    public int maxProjectiles = 5;
    private GameObject towerInstance;
    private Transform firePoint;
    private GameObject projectileContainer;
    private List<GameObject> activeProjectiles = new List<GameObject>();

    private GameObject rangeIndicator;

    private SphereCollider rangeCollider;

    private List<Transform> dogsInRange = new List<Transform>();
    private Transform currentTarget;
    private float rotationSpeed = 5f;

    private Coroutine attackRoutine;

    void Start()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, towerData.towerPrefab.transform.position.y, transform.position.z);
        towerInstance = Instantiate(towerData.towerPrefab, spawnPos, Quaternion.identity, transform);
        towerInstance.AddComponent<TowerHover>().parent = this;
        firePoint = towerInstance.transform.Find("ProjectileDirection");
        if (firePoint == null) firePoint = towerInstance.transform;

        projectileContainer = new GameObject("Projectiles");
        projectileContainer.transform.SetParent(transform);
        projectileContainer.transform.localPosition = Vector3.zero;

        rangeIndicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        rangeIndicator.transform.SetParent(transform);
        rangeIndicator.transform.localPosition = Vector3.zero;
        rangeIndicator.transform.localScale = new Vector3(towerData.attackRadius, 0.01f, towerData.attackRadius);
        rangeIndicator.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.25f);
        Destroy(rangeIndicator.GetComponent<Collider>());
        rangeIndicator.SetActive(true);

        rangeCollider = this.gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = towerData.attackRadius / 2;
    }

    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 lookPos = currentTarget.position - towerInstance.transform.position;
            lookPos.y = 0;

            if (lookPos != Vector3.zero)
                towerInstance.transform.rotation = Quaternion.Slerp(
                    towerInstance.transform.rotation,
                    Quaternion.LookRotation(-lookPos),
                    Time.deltaTime * rotationSpeed
                );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            Debug.Log($"Dog entered range: {other.name}");
            dogsInRange.Add(other.transform);

            if (currentTarget == null)
            {
                currentTarget = GetClosestDog();
                if (attackRoutine == null)
                    attackRoutine = StartCoroutine(AttackTarget());
            }

        }
    }

    IEnumerator AttackTarget()
    {
        while (currentTarget != null)
        {
            // TODO: ADD MULTI TARGET STUFF / AOE THINGS
            FireProjectile(currentTarget);
            yield return new WaitForSeconds(towerData.attackRate);
        }
    }

    void FireProjectile(Transform target)
    {
        if (towerData.projectilePrefab != null)
        {

            GameObject proj = Instantiate(towerData.projectilePrefab, firePoint.position, Quaternion.identity, projectileContainer.transform);
            Debug.Log($"Firing projectile at {target.name}");
            activeProjectiles.Add(proj);
            proj.GetComponent<ProjectileManager>().damageAmount = towerData.damageAmount;
            if (activeProjectiles.Count > maxProjectiles)
            {
                Destroy(activeProjectiles[0]);
                activeProjectiles.RemoveAt(0);
            }

            Rigidbody projRb = proj.GetComponent<Rigidbody>();
            Vector3 targetPos = currentTarget.position;
            Vector3 startPos = firePoint.position;

            Vector3 delta = currentTarget.position - firePoint.position;
            Vector3 deltaXZ = new Vector3(delta.x, 0, delta.z);
            float distance = deltaXZ.magnitude;

            float height = 0f;
            float gravity = -Physics.gravity.y;
            float minHeight = Mathf.Max(height, delta.y + 0.5f);
            float Vy = Mathf.Sqrt(2f * gravity * minHeight);

            float timeUp = Vy / gravity;

            float timeDown = Mathf.Sqrt(2f * (minHeight - delta.y) / gravity);
            float timeTotal = timeUp + timeDown;

            float Vxz = distance / timeTotal;
            Vector3 velocity = deltaXZ.normalized * Vxz + Vector3.up * Vy;
            projRb.linearVelocity = velocity;

            projRb.useGravity = true;

        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dog") && currentTarget == null)
        {
            currentTarget = GetClosestDog();
            if (currentTarget != null && attackRoutine == null)
                attackRoutine = StartCoroutine(AttackTarget());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            Debug.Log($"Dog left range: {other.name}");
            dogsInRange.Remove(other.transform);

            if (other.transform == currentTarget)
            {
                currentTarget = GetClosestDog();
                if (currentTarget == null && attackRoutine != null)
                {
                    StopCoroutine(attackRoutine);
                    attackRoutine = null;
                }
            }
        }
    }

    public void ShowRange(bool show)
    {
        rangeIndicator.SetActive(show);
    }

    private Transform GetClosestDog()
    {
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Transform dog in dogsInRange)
        {
            if (dog == null) continue;
            float dist = Vector3.Distance(transform.position, dog.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = dog;
            }
        }
        return closest;
    }

    public void RemoveDog(Transform dog)
    {
        dogsInRange.Remove(dog);
        if (dog == currentTarget)
            currentTarget = GetClosestDog();
    }


}
