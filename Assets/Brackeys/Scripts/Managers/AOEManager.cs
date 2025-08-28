using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AOEManager : MonoBehaviour
{
    private float timer;
    private Dictionary<DogManager, Coroutine> activeDogs = new Dictionary<DogManager, Coroutine>();
    public TowerScriptableObject towerData;
    void Start()
    {
        timer = towerData.damageDuration;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            foreach (DogManager dog in activeDogs.Keys)
            {
                if (dog != null)
                    dog.ResetSpeed();
            }
            foreach (var c in activeDogs.Values)
            {
                if (c != null)
                    StopCoroutine(c);
            }
            activeDogs.Clear();
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        DogManager dog = other.GetComponentInParent<DogManager>();
        if (dog != null)
        {
            dog.ModifySpeed(towerData.enemySpeed);
            if (!activeDogs.ContainsKey(dog))
            {
                Coroutine c = StartCoroutine(ApplyDamageOverTime(dog));
                activeDogs.Add(dog, c);
            }

        }
    }

    void OnTriggerStay(Collider other)
    {
        DogManager dog = other.GetComponentInParent<DogManager>();
        if (dog != null)
        {
            dog.ModifySpeed(towerData.enemySpeed);
            if (!activeDogs.ContainsKey(dog))
            {
                Coroutine c = StartCoroutine(ApplyDamageOverTime(dog));
                activeDogs.Add(dog, c);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        DogManager dog = other.GetComponentInParent<DogManager>();
        if (dog != null)
        {
            dog.ResetSpeed();
            if (activeDogs.ContainsKey(dog))
            {
                StopCoroutine(activeDogs[dog]);
                activeDogs.Remove(dog);
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(DogManager dog)
    {
        while (dog != null)
        {
            dog.TakeDamage(towerData.damageAmount);
            yield return new WaitForSeconds(1f); // damage every second
        }
    }
}
