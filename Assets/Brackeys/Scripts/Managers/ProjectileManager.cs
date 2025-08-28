using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public float lifetime = 5f;
    private float timer;
    public bool isAOEProjectile = false;
    public GameObject aoeObject;
    public TowerScriptableObject towerData;
    void Start()
    {
        timer = lifetime;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (isAOEProjectile)
        {
            if (other.CompareTag("Dog") || other.CompareTag("Ground") || other.CompareTag("Path"))
            {
                if (aoeObject != null)
                {
                    Vector3 spawnPos = other.transform.position;
                    spawnPos.y = 0f;
                    GameObject aoe = Instantiate(aoeObject, spawnPos, Quaternion.identity);
                    AOEManager aoeManager = aoe.GetComponent<AOEManager>();
                    if (aoeManager != null)
                    {
                        aoeManager.towerData = towerData;
                    }

                }

                Destroy(gameObject);
            }
        }
        else
        {
            DogManager dog = other.GetComponentInParent<DogManager>();
            if (dog != null)
            {
                dog.TakeDamage(towerData.damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
