using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public float lifetime = 5f;
    private float timer;

    public float damageAmount = 1f;

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
        DogManager dog = other.GetComponentInParent<DogManager>();
        if (dog != null)
        {
            Debug.Log($"Projectile hit {dog.dogData.dogName}, dealing {damageAmount} damage.");
            dog.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
