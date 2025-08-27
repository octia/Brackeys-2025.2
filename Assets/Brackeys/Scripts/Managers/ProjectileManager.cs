using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public float damageAmount = 1f;


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
