using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;

public class TowerManager : MonoBehaviour
{
    public int currentLevel = 0;
    [SerializeField] bool spawnOnStart;

    public GameObject builderVisual;

    [HideInInspector] public TowerScriptableObject towerData;
    private GameObject towerInstance;
    private Animator towerAnimator;
    private ParticleSystem towerParticles;
    private Transform firePoint;
    private GameObject projectileContainer;

    private GameObject rangeIndicator;

    private SphereCollider rangeCollider;

    public List<Transform> dogsInRange = new List<Transform>();
    public Transform currentTarget;
    private float rotationSpeed = 5f;

    public float attackCooldown;
    public float currentHealth;
    public float maxHealth;
    private float lastRadius = 0;
    public bool abilityActive = false;
    private bool isAttacking = false;


    private TowerUI towerUI;

    [Inject]
    private TimerManager timerManager;
    [HideInInspector] public bool spawned;

    [Inject]
    private BiscuitManager biscuitManager;

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

        if (towerData.levels[currentLevel].attackRadius != lastRadius)
        {
            lastRadius = towerData.levels[currentLevel].attackRadius;
            rangeCollider.radius = lastRadius / 2f;
            rangeIndicator.transform.localScale = new Vector3(lastRadius, 0.01f, lastRadius);
        }
        if (towerData.levels[currentLevel].towerHealth != maxHealth)
        {
            maxHealth = towerData.levels[currentLevel].towerHealth;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        if (!timerManager.IsPaused)
        {

            if (currentTarget != null)
            {
                if (!currentTarget.CompareTag("Dog"))
                {
                    currentTarget = GetClosestDog();
                }
                else if (towerData.shouldLookAtTarget && currentHealth > 0)
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

            attackCooldown += Time.deltaTime;
            currentHealth -= Time.deltaTime;
            towerUI.UpdateCooldown(attackCooldown / (towerData.levels[currentLevel].attackRate + towerData.animationDelay));
            towerUI.UpdateHealth(currentHealth / towerData.levels[currentLevel].towerHealth);

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            else
            {
                if (biscuitManager.Biscuit - Mathf.RoundToInt(towerData.levels[currentLevel].attackCost) < 1)
                {
                    return;
                }

                if (attackCooldown >= towerData.levels[currentLevel].attackRate && currentTarget != null && !isAttacking)
                {
                    biscuitManager.Biscuit -= Mathf.RoundToInt(towerData.levels[currentLevel].attackCost);
                    attackCooldown = towerData.levels[currentLevel].attackRate;
                    towerAnimator.Play("Attack");
                    if (towerParticles)
                    {
                        towerParticles.Play();
                    }
                    isAttacking = true;
                }
                if (attackCooldown >= towerData.levels[currentLevel].attackRate + towerData.animationDelay)
                {
                    attackCooldown = towerData.levels[currentLevel].attackRate;
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
                        isAttacking = false;

                    }
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
        towerAnimator = towerInstance.GetComponentInChildren<Animator>();
        towerParticles = towerInstance.GetComponentInChildren<ParticleSystem>();

        maxHealth = towerData.levels[currentLevel].towerHealth;
        currentHealth = maxHealth;
        if (towerParticles)
        {
            towerParticles.Stop();
        }
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
        rangeIndicator.SetActive(false);

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
        if (towerData.projectilePrefab == null) return;
        if (target == null || !target.CompareTag("Dog") || !dogsInRange.Contains(target)) return;

        //towerAnimator.SetTrigger("Attack");

        GameObject proj = Instantiate(towerData.projectilePrefab, firePoint.position, Quaternion.identity, projectileContainer.transform);
        proj.GetComponent<ProjectileManager>().towerData = towerData;
        proj.GetComponent<ProjectileManager>().currentLevel = currentLevel;
        Rigidbody projRb = proj.GetComponent<Rigidbody>();

        Vector3 startPos = firePoint.position;
        Vector3 targetPos = currentTarget.position;
        Vector3 targetVel = currentTarget.GetComponentInParent<DogManager>().currentVelocity;

        Vector3 deltaInitial = targetPos - startPos;
        float height = towerData.levels[currentLevel].attackHeight;
        float gravity = -Physics.gravity.y;
        float minHeight = Mathf.Max(height, deltaInitial.y + 0.5f);
        float Vy = Mathf.Sqrt(2f * gravity * minHeight);
        float timeUp = Vy / gravity;
        float timeDown = Mathf.Sqrt(2f * (minHeight - deltaInitial.y) / gravity);
        float timeTotal = timeUp + timeDown;

        Vector3 predictedPos = targetPos + targetVel * timeTotal;

        Vector3 delta = predictedPos - startPos;
        Vector3 deltaXZ = new Vector3(delta.x, 0, delta.z);
        float distance = deltaXZ.magnitude;
        minHeight = Mathf.Max(height, delta.y + 0.5f);
        Vy = Mathf.Sqrt(2f * gravity * minHeight);
        timeUp = Vy / gravity;
        timeDown = Mathf.Sqrt(2f * (minHeight - delta.y) / gravity);
        timeTotal = timeUp + timeDown;

        float Vxz = distance / timeTotal;
        Vector3 velocity = deltaXZ.normalized * Vxz + Vector3.up * Vy;
        projRb.linearVelocity = velocity;
        projRb.useGravity = true;
    }

    private IEnumerator AOEAbilityCoroutine()
    {
        float elapsed = 0f;
        float tickRate = 1f;

        while (elapsed < towerData.levels[currentLevel].damageDuration)
        {
            abilityActive = true;
            foreach (Transform dog in dogsInRange)
            {
                if (dog != null)
                {
                    dog.GetComponentInParent<DogManager>().ModifySpeed(towerData.levels[currentLevel].enemySpeed);
                    dog.GetComponentInParent<DogManager>().TakeDamage(towerData.levels[currentLevel].damageAmount);
                }
            }

            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }

        attackCooldown = 0;
        abilityActive = false;
        towerAnimator.Play("Idle");
        if (towerParticles)
        {
            towerParticles.Stop();
        }

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
            if (!dog.CompareTag("Dog")) continue;
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
        dogsInRange.RemoveAll(d => d == null || d == dog || !d.CompareTag("Dog"));
        if (dog == currentTarget)
            currentTarget = GetClosestDog();
    }

    public void UpgradeLevel()
    {
        if (currentLevel + 1 < towerData.levels.Count)
        {
            currentLevel++;
        }
    }

    public void RepairHealth()
    {
        currentHealth = maxHealth;
    }
}
