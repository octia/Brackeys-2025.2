using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    [SerializeField] bool spawnOnStart;

    public GameObject builderVisual;
    
    [HideInInspector] public TowerScriptableObject towerData;
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
    public bool shouldLookAtTarget = true;

    private TowerUI towerUI;

    // ...
    [HideInInspector] public bool spawned;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnTower();
        }
    }

    void Update()
    {
        if (!spawned)
            return;

        if (towerData.attackRadius != lastRadius)
        {
            lastRadius = towerData.attackRadius;
            rangeCollider.radius = lastRadius / 2f;
            rangeIndicator.transform.localScale = new Vector3(lastRadius, 0.01f, lastRadius);
        }

        if (currentTarget != null && shouldLookAtTarget)
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
        towerUI.UpdateCooldown(attackCooldown / towerData.attackRate);
        if (attackCooldown >= towerData.attackRate)
        {
            attackCooldown = towerData.attackRate;
            if (currentTarget != null)
            {
                attackCooldown = 0;
                if (towerData.isAttackAOE)
                {
                    StartCoroutine(AOEAbilityCoroutine());
                }
                else
                {
                    FireProjectile(currentTarget);
                }
            }
        }
    }

    public void SpawnTower()
    {
        spawned = true;

        Vector3 spawnPos = new Vector3(transform.position.x, towerData.towerPrefab.transform.position.y, transform.position.z);
        towerInstance = Instantiate(towerData.towerPrefab, spawnPos, Quaternion.identity, transform);
        towerInstance.AddComponent<TowerHover>().parent = this;
        towerUI = towerInstance.transform.Find("UI_Canvas").GetComponent<TowerUI>();

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
        rangeIndicator.GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f, 0.15f);
        Destroy(rangeIndicator.GetComponent<Collider>());
        rangeIndicator.SetActive(true);

        rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!spawned)
            return;

        if (other.CompareTag("Dog"))
        {
            dogsInRange.Add(other.transform);

            if (currentTarget == null)
            {
                currentTarget = GetClosestDog();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!spawned)
            return;

        if (other.CompareTag("Dog") && currentTarget == null)
        {
            currentTarget = GetClosestDog();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!spawned)
            return;

        if (other.CompareTag("Dog"))
        {
            dogsInRange.Remove(other.transform);

            if (other.transform == currentTarget)
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
            proj.GetComponent<ProjectileManager>().towerData = towerData;


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
        dogsInRange.RemoveAll(d => d == null || d == dog);
        if (dog == currentTarget)
            currentTarget = GetClosestDog();
    }
}
