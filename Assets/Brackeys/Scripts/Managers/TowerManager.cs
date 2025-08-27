using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public TowerScriptableObject towerData;
    private GameObject towerInstance;
    private Transform firePoint;
    private GameObject projectileContainer;

    private GameObject rangeIndicator;

    private SphereCollider rangeCollider;

    public List<Transform> dogsInRange = new List<Transform>();
    public Transform currentTarget;
    private float rotationSpeed = 5f;

    public float attackCooldown;
    private float lastRadius = 0;
    public bool abilityActive = false;

    void Start()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, towerData.towerPrefab.transform.position.y, transform.position.z);
        towerInstance = Instantiate(towerData.towerPrefab, spawnPos, Quaternion.identity, transform);
        towerInstance.AddComponent<TowerHover>().parent = this;

        if (towerData.isAttackAOE)
        {

        }
        else
        {
            firePoint = towerInstance.transform.Find("ProjectileDirection");
            if (firePoint == null) firePoint = towerInstance.transform;
            projectileContainer = new GameObject("Projectiles");
            projectileContainer.transform.SetParent(transform);
            projectileContainer.transform.localPosition = Vector3.zero;
        }


        rangeIndicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        rangeIndicator.transform.SetParent(transform);
        rangeIndicator.transform.localPosition = Vector3.zero;
        rangeIndicator.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.25f);
        Destroy(rangeIndicator.GetComponent<Collider>());
        rangeIndicator.SetActive(true);

        rangeCollider = this.gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
    }

    void Update()
    {
        if (towerData.attackRadius != lastRadius)
        {
            lastRadius = towerData.attackRadius;
            rangeCollider.radius = lastRadius / 2f;
            rangeIndicator.transform.localScale = new Vector3(lastRadius, 0.01f, lastRadius);
        }
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

        attackCooldown += Time.deltaTime;

        if (attackCooldown >= towerData.attackRate)
        {
            attackCooldown = 0;
            if (currentTarget != null && !towerData.isAttackAOE)
                FireProjectile(currentTarget);
            else
                StartCoroutine(AOEAbilityCoroutine());
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
            }
        }
    }

    void FireProjectile(Transform target)
    {
        if (towerData.projectilePrefab != null)
        {

            GameObject proj = Instantiate(towerData.projectilePrefab, firePoint.position, Quaternion.identity, projectileContainer.transform);
            Debug.Log($"Firing projectile at {target.name} from {this.name}");
            proj.GetComponent<ProjectileManager>().damageAmount = towerData.damageAmount;


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

    private IEnumerator AOEAbilityCoroutine()
    {
        float elapsed = 0f;
        float tickRate = 1f;

        while (elapsed < towerData.damageDuration)
        {
            abilityActive = true;
            foreach (Transform dog in dogsInRange)
            {
                if (dog != null)
                {
                    dog.GetComponentInParent<DogManager>().ModifySpeed(towerData.enemySpeed);   // apply slow
                    dog.GetComponentInParent<DogManager>().TakeDamage(towerData.damageAmount); // apply tick damage
                }
            }

            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }

        attackCooldown = 0;
        abilityActive = false;

        // Reset speeds at the end
        foreach (Transform dog in dogsInRange)
        {
            if (dog != null) dog.GetComponentInParent<DogManager>().ResetSpeed();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dog") && currentTarget == null)
        {
            currentTarget = GetClosestDog();
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
        Debug.Log("CALLING TO REMOVE DOG FROM LIST");
        dogsInRange.Remove(dog);
        if (dog == currentTarget)
            currentTarget = GetClosestDog();
    }

}
