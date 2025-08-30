using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveSpawner : MonoBehaviour
{
    public List<WaveScriptableObject> waves;
    public Transform spawnPoint;
    //public Transform dogPrefab;

    public float timeBetweenWaves = 5f;
    public float countdown = 2f;

    public int waveIndex = 0;

    [Inject]
    public TimerManager timerManager;

    [Inject]
    public BiscuitManager biscuitManager;

    void Start()
    {
        StartCoroutine(RunWaves());
    }
    private IEnumerator RunWaves()
    {
        while (waveIndex < waves.Count)
        {
            WaveScriptableObject wave = waves[waveIndex];
            Debug.Log($"Starting wave: {wave.waveName}");

            List<DogScriptableBehaviour> spawnPool = new List<DogScriptableBehaviour>();
            foreach (DogSpawnEntry entry in wave.dogs)
            {
                for (int i = 0; i < entry.count; i++)
                {
                    spawnPool.Add(entry.dogType);
                }
            }

            while (spawnPool.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnPool.Count);
                DogScriptableBehaviour dog = spawnPool[randomIndex];
                spawnPool.RemoveAt(randomIndex);

                GameObject dogGO = new GameObject(dog.dogName);
                DogManager dogManager = dogGO.AddComponent<DogManager>();
                dogManager.dogData = dog;
                dogManager.waypointLineIndex = Random.Range(0, Waypoints.pointsList.Count);
                dogManager.timerManager = timerManager;
                dogManager.biscuitManager = biscuitManager;

                dogGO.transform.SetParent(Waypoints.pointsList[dogManager.waypointLineIndex][0], false);
                dogGO.transform.localPosition = Vector3.zero;
                dogGO.transform.localRotation = Quaternion.identity;


                float waitTime = Random.Range(waves[waveIndex].spawnRateRange.x, waves[waveIndex].spawnRateRange.y);
                yield return new WaitForSeconds(waitTime);
            }

            yield return new WaitForSeconds(timeBetweenWaves);
            waveIndex++;
        }

        Debug.Log("All waves complete!");
    }

}
